using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S4M.Query
{
    class Query
    {
        public static string Query_ZLP_DokMag(string dokmag, string kod, string data)
        {
            string kodjest = kod.Length > 0 ? "and tw.kod like '%" + kod + "%' " : "";
            string query = "set dateformat ymd; set nocount on; " +
                "SELECT DISTINCT " +
                "convert(varchar, mg.data,23) as 'Data' " +
                ", DW.ID AS IdDostawy, mg.typ_dk as TYP " +
                ", mg.kod as NrDokumentu " +
                ", mg.opis as OpisDokumentu " +
                ", tw.kod as KodTowaru " +
                ", tw.nazwa as NazwaTowaru " +
                ", dw.kod as PartiaTW " +
                ", case " +
                "when mg.opis like '%Zwrot%' then pw.ilosc " +
                "else -pw.ilosc " +
                "END as 'Ilość' " +
                ", mz.jm as JM " +
                ", ss.CDim_LiczbaKartonów as Kartony " +
                ", ss.CDim_PARTIADOSTAWCY as PartiaSurowca " +
                ", ss.Cdim_Lot as Lot " +
                ", ss.CDim_SSCC_ESPERA as 'SSCC Espera' " +
                ", ss.cDim_dataprodukcji as 'Data Produkcji' " +
                ", ss.cDim_NumerJednostki as 'Numer Jednostki' " +
                ", ss.cdim_CentrumDystrybucyjne_Val as 'Centrum' " +
                ", kontrah.kod as 'Kontrahent' " +
                "FROM hm.mg mg " +
                "JOIN hm.MZ mz on mz.super= mg.id " +
                "JOIN hm.PW pw on pw.typ= 37 and pw.iddkmg= mg.id and pw.idtw= mz.idtw " +
                "JOIN hm.dw dw on dw.id= pw.iddw " +
                "join hm.tw tw on dw.idtw= tw.id " +
                "LEFT JOIN mirkoerp.SSCommon.HMF_WarehouseDeliveryClassification ss on ss.ElementId = dw.id " +
                "left join mirkoerp.sscommon.vkontrahenci kontrah on kontrah.id = mg.khid " +
                "where mg.data >= '" + data + "' and (mg.typ_dk like 'PP' or mg.typ_dk like 'WP' or mg.typ_dk like 'PPK' or mg.typ_dk like 'WPK') " +
                "and (mg.opis like '%" + dokmag + "%') " +
                kodjest +
                "order by OpisDokumentu,dw.kod ";
            return query;
        }

        // -----------------------------------------------------------------------------------------------------------------
        public static string Query_HM_MG_A(string partia,string kod)
        {
            string query = "SELECT distinct mg.typ_dk as TypDokument , mg.kod as 'Numer Dokumentu', " +
                "ss.kod as 'Nazwa Kontrahenta', " +
                "tw.kod as KodTowaru, " +
                "tw.nazwa as NazwaTowaru," +
                "dw.kod as 'Partia Surowca Mirko'," +
                "abs(pw.ilosc) as 'Ilość Przyjęta do Mirko', " +
                "dw.id as IDDostawy " +
                "FROM hm.mg mg " +
                "JOIN hm.mz mz on mz.super = mg.id " +
                "JOIN hm.PW pw on pw.typ = 37 and pw.iddkmg = mg.id and pw.idtw = mz.idtw " +
                "JOIN hm.dw dw on dw.id = pw.iddw " +
                "JOIN hm.tw tw on dw.idtw = tw.id " +
                "LEFT JOIN mirkoerp.sscommon.vKontrahenci ss on dw.idkh = ss.id " +
                "WHERE mg.data >= '2020-01-01' and mg.typ_dk like 'PZ' and tw.kod like '" + kod + "' and dw.kod like '%" + partia + "%'"; //'%19/05/20/GS%'";
            return query;
        }
        //-- tabela iddostawy pierwotnej w celu powiązania dostawy z PZ z dokumentami WP do marynowania
        public static string Query_PZ_Table(string partia, string kod)
        {
            string query = " declare @PZ table(IDDostawy int) " +
                "insert into @PZ " +
                "SELECT isnull(dw.iddw, dw.id) as IDDostawyPierwotnej " +
                "FROM hm.mg mg " +
                "JOIN hm.MZ mz on mz.super= mg.id " +
                "JOIN hm.PW pw on pw.typ = 37 and pw.iddkmg = mg.id and pw.idtw = mz.idtw " +
                "JOIN hm.dw dw on dw.id = pw.iddw " +
                "join hm.tw tw on dw.idtw = tw.id " +
                "left join mirkoerp.sscommon.vKontrahenci ss on dw.idkh = ss.id " +
                "where mg.data >= '2020-01-01' and mg.typ_dk like 'PZ' and tw.kod like '" + kod + "' and dw.kod like '%" + partia + "%' ";
            return query;
        }
        //--tabela numerów zleceń do powiązania PP z WP na rybę.
        public static string Query_Maynowanie_Table(string partia, string kod)
        {
            string query = " declare @Marynowanie table(NRZlecenia varchar(20)) " +
                "insert into @Marynowanie " +
                "SELECT left(mg.opis,11) as OpisDokumentu " +
                "FROM hm.mg mg " +
                "JOIN hm.MZ mz on mz.super = mg.id " +
                "JOIN hm.PW pw on pw.typ = 37 and pw.iddkmg = mg.id and pw.idtw = mz.idtw " +
                "JOIN hm.dw dw on dw.id = pw.iddw " +
                "join hm.tw tw on dw.idtw = tw.id " +
                "where mg.data >= '2020-01-01' and mg.typ_dk like 'WP' and tw.kod like '" + kod + "' and isnull(dw.iddw, dw.id) in (select* from @PZ) and dw.kod like '%" + partia + "%' " +
                "order by left(mg.opis,11) ";
            return query;
        }

        //--tabela dokumentów PP z marynowania na podstawie numerów zleceń z pobrania WP

        public static string Query_PP_WP(string partia, string kod)
        {
            string query = "SELECT mg.typ_dk as TypDokumentu, tw.kod as 'Kod Towaru Mirko', tw.nazwa as 'Nazwa Towaru Mirko', dw.kod as 'Partia Towaru Mirko', sum(abs(pw.ilosc)) as 'Ilość Ryby Zamarynowanej/Solonej' " +
                "FROM hm.mg mg " +
                "JOIN hm.MZ mz on mz.super = mg.id " +
                "JOIN hm.PW pw on pw.typ = 37 and pw.iddkmg = mg.id and pw.idtw = mz.idtw " +
                "JOIN hm.dw dw on dw.id = pw.iddw " +
                "join hm.tw tw on dw.idtw = tw.id " +
                "where mg.data >= '2020-01-01' and (mg.typ_dk like 'pp') and tw.kod like '100-%' and left(mg.opis,11) in (select* from @Marynowanie) and dw.kod like '%" + partia + "%' " +
                "group by tw.kod, dw.kod, tw.nazwa, mg.typ_dk " +
                "order by dw.kod ";
            return query;
        }

        // -- tabela idpierwotnejdostawy utworzona ze zleceń marynowania, do tabeli dodane iddostawy z dokumentów PW w celu uchwycenia wszystkich partii użytych do produkcji
        public static string Query_Ryba_Table(string partia, string kod)
        {
            string query = "declare @Ryba table(IDDostawy int) " +
                "insert into @Ryba " +
                "SELECT isnull(dw.iddw, dw.id) as IDDostawyPierwotnej " +
                "FROM hm.mg mg " +
                "JOIN hm.MZ mz on mz.super= mg.id " +
                "JOIN hm.PW pw on pw.typ= 37 and pw.iddkmg= mg.id and pw.idtw= mz.idtw " +
                "JOIN hm.dw dw on dw.id= pw.iddw " +
                "join hm.tw tw on dw.idtw= tw.id " +
                "where mg.data >= '2020-01-01' and mg.typ_dk like 'pp' and tw.kod like '100-%' and left(mg.opis,11) in (select* from @Marynowanie) and dw.kod like '%" + partia + "%' " +
                "order by left(mg.opis,11) " +
                
                "insert into @Ryba " +
                "SELECT isnull(dw.iddw, dw.id) as IDDostawyPierwotnej " +
                "FROM hm.mg mg " +
                "JOIN hm.MZ mz on mz.super= mg.id " +
                "JOIN hm.PW pw on pw.typ= 37 and pw.iddkmg= mg.id and pw.idtw= mz.idtw " +
                "JOIN hm.dw dw on dw.id= pw.iddw " +
                "join hm.tw tw on dw.idtw= tw.id " +
                "where mg.data >= '2020-01-01' and mg.typ_dk like 'pw' and tw.kod like '100-%' and dw.kod like '%" + partia + "%' ";
            return query;
        }

        //--tabela dokumentów WP użytej ryby do zleceń produkcyjnych.
        public static string Query_WP_Ryba(string partia, string kod)
        {
            string query = "SELECT tw.kod as 'Kod Towaru Mirko', tw.nazwa as 'Nazwa Towaru Mirko', dw.kod as 'Partia Towaru Mirko', sum(abs(pw.ilosc)) as 'Ilość Surowca Użyta Do Produkcji', left(mg.opis,11) as 'Numer Zlecenia Produkcyjnego' " +
                "FROM hm.mg mg " +
                "JOIN hm.MZ mz on mz.super= mg.id " +
                "JOIN hm.PW pw on pw.typ= 37 and pw.iddkmg= mg.id and pw.idtw= mz.idtw " +
                "JOIN hm.dw dw on dw.id= pw.iddw " +
                "join hm.tw tw on dw.idtw= tw.id " +
                "where mg.data >= '2020-01-01' and mg.typ_dk like 'WP' and tw.kod like '100-%' and dw.kod like '%" + partia + "%' and isnull(dw.iddw, dw.id) in (select* from @Ryba) " +
                "group by left(mg.opis,11), tw.kod, dw.kod, tw.nazwa order by dw.kod ";
            return query;
        }

        //--tabela numerów zleceń produkcyjnych w celu powiązania WP na rybę z PP na wyrób gotowy

        public static string Query_ZlPWG_Table(string partia, string kod)
        {
            string query = "declare @ZlPWG table(NrZlecenia varchar(20)) " +
                "insert into @ZLPWG " +
                "SELECT left(mg.opis,11) as OpisDokumentu " +
                "FROM hm.mg mg " +
                "JOIN hm.MZ mz on mz.super= mg.id " +
                "JOIN hm.PW pw on pw.typ= 37 and pw.iddkmg= mg.id and pw.idtw= mz.idtw " +
                "JOIN hm.dw dw on dw.id= pw.iddw " +
                "join hm.tw tw on dw.idtw= tw.id " +
                "where mg.data >= '2020-01-01' and mg.typ_dk like 'WP' and tw.kod like '100-%' and dw.kod like '%" + partia + "%' ";
            return query;
        }

        //--tabela PP na wyrób gotowy
        public static string Query_PP_WyrobGotowy(string partia, string kod)
        {
            string query = "SELECT left(mg.opis,11) as 'Numer Zlecenia Produkcyjnego', tw.kod as 'Kod Wyrobu Mirko', tw.nazwa as 'Nazwa Wyrobu Mirko', dw.kod as 'Partia Wyrobu Mirko', sum(abs(pw.ilosc)) as 'Ilość Przyjęta z Produkcji' " +
                "FROM hm.mg mg " +
                "JOIN hm.MZ mz on mz.super= mg.id " +
                "JOIN hm.PW pw on pw.typ= 37 and pw.iddkmg= mg.id and pw.idtw= mz.idtw " +
                "JOIN hm.dw dw on dw.id= pw.iddw " +
                "join hm.tw tw on dw.idtw= tw.id " +
                "where mg.data >= '2020-01-01' and mg.typ_dk like 'PP' and tw.kod like '200-%' and left(mg.opis,11) in (select* from @ZLPWG) " +
                "group by left(mg.opis,11), tw.kod , dw.kod, tw.nazwa ";
            return query;
        }

        //--tabela iddostawypierwotnej wyrobów gotowych w celu powiązania z dokumentami WZ
        public static string Query_ZLPWZ_Table(string partia, string kod)
        {
            string query = "declare @ZLPWZ table(NumerDostawy int) " +
                "insert into @ZLPWZ " +
                "SELECT isnull(dw.iddw, dw.id) as IDDostawyPierwotnej " +
                "FROM hm.mg mg " +
                "JOIN hm.MZ mz on mz.super= mg.id " +
                "JOIN hm.PW pw on pw.typ= 37 and pw.iddkmg= mg.id and pw.idtw= mz.idtw " +
                "JOIN hm.dw dw on dw.id= pw.iddw " +
                "join hm.tw tw on dw.idtw= tw.id " +
                "where mg.data >= '2020-01-01' and mg.typ_dk like 'PP' and tw.kod like '200-%' and left(mg.opis,11) in (select* from @ZLPWG) ";
            return query;
        }

        //--tabela dokumentów WZ w celu wskazania, gdzie wyjechały towary
        public static string Query_WZ_Odbiorca(string partia, string kod)
        {
            string query = "SELECT mg.kod as 'Numer Dokumentu Mirko', mg.opis as 'Opis Dokumentu Mirko', tw.kod as 'Kod Wyrobu Mirko', tw.nazwa as 'Nazwa Wyrobu Mirko', dw.kod as 'Partia Wyrobu Mirko', abs(pw.ilosc) as 'Ilość wskazana na Dokumencie Wydania', ss.kod as 'Odbiorca' " +
                "FROM hm.mg mg " +
                "JOIN hm.MZ mz on mz.super= mg.id " +
                "JOIN hm.PW pw on pw.typ= 37 and pw.iddkmg= mg.id and pw.idtw= mz.idtw " +
                "JOIN hm.dw dw on dw.id= pw.iddw " +
                "join hm.tw tw on dw.idtw= tw.id " +
                "left join mirkoerp.SSCommon.vKontrahenci ss on ss.id = mg.khid " +
                "where mg.data >= '2020-01-01' and (mg.typ_dk like 'WZ' or mg.typ_dk like 'WZK') and tw.kod like '200-%' and isnull(dw.iddw, dw.id) in (select* from @ZLPWZ) ";
            return query;
        }


        // -----------------------------------------------------------------------------------------------------------------
        public static string Query_HM_ZO(string data)
        {
            string query = "SELECT TOP(999) id, flag, aktywny, subtyp, typ, znacznik, rodzaj, katalog, info, kod, seria, serianr, okres, seriadzial, nazwa, data, datasp, opis, khid, khadid, odid, odadid, ok, wplaty, rabat, plattyp, plattermin, netto, vat, odebrane, typ_dk, "
          + "grupacen, iddokkoryg, wartoscSp, exp_fk, waluta, kurs, zyskdod, paragon, kod_obcy, magazyn, rozlmg, formaplatn, schemat, datarej, bufor, super, anulowany, wystawil, createdBy, createdDate, modifiedBy, modifiedDate, rr, "
            + "datawplywu, walNetto, walBrutto, statusRDF, guid, typceny, splitPayment, statusMig, zeroVatRate FROM HM.ZO WHERE (data = '" + data + "') ORDER BY id DESC";
            return query;
        }

        public static string Query_MF_PR(string data)
        {
            string query = "SELECT TOP(20000) ID, ReceiptCode, ReceiptType, ReceiptName, ItemID, Qty, UoM, UoMSelected, Workhours, WorkUnit, UnitPrice, DestSiteID, DepartmentID, WorkerID, ReceiptGroup, Status, PriceCalculateType, MaterialFlushMethod, " +
                         "IsExternal, Comments, Picture, Revision, QTYPrecision, SalePriceFactor, Charge, LotSize, CreatedBy, CreateDate, ModifiedBy, ModifiedDate, ParentReceiptDelay, IsDirty, ParentReceiptDelayType, OptimisticLockField " +
                         "FROM  MF.ProductionReceipt ORDER BY ID";
            return query;
        }

        public static string Query_MF_PRB(string id)
        {
            string query = "SELECT MF.ProductionReceiptBOM.LP,HM.TW.kod, HM.TW.nazwa, MF.ProductionReceiptBOM.ItemID, MF.ProductionReceiptBOM.Sequence, MF.ProductionReceiptBOM.ItemQTY, " +
                        "MF.ProductionReceiptBOM.ItemUoM, MF.ProductionReceiptBOM.ItemPrice, MF.ProductionReceiptBOM.SourceSiteID, MF.ProductionReceiptBOM.ScrapPercentage, " +
                        "MF.ProductionReceiptBOM.OptimisticLockField " +
                       "FROM MF.ProductionReceiptBOM INNER JOIN HM.TW ON MF.ProductionReceiptBOM.ItemID = HM.TW.id " +
                       "WHERE ( MF.ProductionReceiptBOM.ReceiptID = " + id + " )";
            return query;
        }

        public static string Query_HM_ZO_Sped(string data)
        {
            string query = "select 'MIRKOSLUGL' as 'Cosignor ISN / SN'"
 // + "--,'TUTAJ DODAĆ KOD' as 'Spedytor'"
 + ", 'MIRKO SP. Z O.O.' as 'Cosignor name'"
 + ", 'GLOBINO' as 'Cosignor city'"
 + ", '76-200' as 'Cosignor postal code'"
 + ", 'PL' as 'Cosignor country'"
 + ", 'PRZEMYSLOWA 12' as 'Cosignor address'"
 + ", NULL as 'Cosignor NIP'"
 + ", NULL as 'Cosignor contact person'"
 + ", NULL as 'Cosignor phone'"
 + ", NULL as 'Cosignor e-mail'"
 + ", 'MIRKOSLUG5' as 'Loading place ISN / SN'"
 + ", 'MIRKO SP. Z O.O.' as 'Loading place name'"
 + ", 'SLUPSK' as 'Loading place city'"
 + ", '76-200' as 'Loading place postal code'"
 + ", 'PL' as 'Loading place country'"
 + ", 'PRZEMYSLOWA 12' as 'Loading place address'"
 + ", NULL as 'Loading place NIP'"
 + ", NULL as 'Loading place contact person'"
 + ", NULL as 'Loading phone'"
 + ", NULL as 'Loading e-mail'"
 + ", NULL as 'Cosignee ISN / SN'"
 + ", NULL as 'Cosignee name'"
 + ", NULL as 'Cosignee city'"
 + ", NULL as 'Cosignee postal code'"
 + ", NULL as 'Cosignee country'"
 + ", NULL as 'Cosignee address'"
 + ", NULL as 'Cosignee NIP'"
 + ", NULL as 'Cosignee contact person'"
 + ", NULL as 'Cosignee phone'"
 + ", NULL as 'Cosignee e-mail'"
 + ", odb.id as 'Unloading place ISN / SN'" //-- Id kontrahenta z bazy MIRKO"
 + ", odb.kod as 'Unloading place name'" //-- Nazwa kontrahenta z bazy MIRKO"
 + ", odb.miejscowosc as 'Unloading place city'" //-- Miejscowość kontrahenta z bazy MIRKO"
 + ", odb.kodpocz as 'Unloading place postal code'" //-- Kod pocztowy kontrahenta z bazy MIRKO"
 + ", odb.idKraju as 'Unloading place country'" //-- ID kraju kontrahenta z bazy MIRKO"
 + ", 'DODAĆ KOD SQL' as 'Unloading place address'" //-- Ulica kontrahenta- do ustalenia miejsce z którego należy pobrać dane"
 + ", NULL as 'Unloading place NIP'"
 + ", NULL as 'Unloading place contact person'"
 + ", NULL as 'Unloading place phone'"
 + ", NULL as 'Unloading place e-mail'"
 + ", zo.data as 'Planned loading date'"
 + ", soc.CDim_DataDostawy as 'Planned delivery date'"
 + ", NULL as 'Customer reference no.'"
 + ", 'ff02' as 'Goodscharacteristic'"
 + ", 'ep' as 'Transport unit type'"
 + ", soc.CDim_LiczbaPalet as 'Quantity of transport units'"
 + ", NULL as 'Length'"
 + ", NULL as 'Width'"
 + ", NULL as 'Height'"
 + ", '?' as 'Gross weight'" //-- Jak to wyliczać ?"
 + ", NULL as 'Volume'"
 + ", NULL as 'Pallet places'"
 + ", 'FP' as 'Securing features'"
 + ", 'Artykuły spożywcze' as 'Goods Description'"
 + ", NULL as 'Shipment ID'"
 + ", NULL as 'Stackable'"
 + ", zo.opis as 'Recipient order no.'" //-- numer zamówienia klienta pobierany z opisu zamówienia! Do ustalenia czy dziewczyny nie wpisują dodatkowych informacji"
 + ", 1 as 'Return of documents (ROD)'"
 + ", replace(zo.kod, '/', ' ') as 'Attached documents no.'"
 + ", NULL as 'Return of Pellets (ROP)'"
 + ", NULL as 'Quantity (ROP)'"
 + ", NULL as 'Loading instructions'"
 + ", NULL as 'Unloading instructions'"
 + ", NULL as 'Project name'"
 + ", CASE DATENAME(dw, soc.CDim_DataDostawy) WHEN 'Sobota' THEN 1 ELSE NULL END as 'Saturday Delivery (SAT)'"
 + ", CASE DATENAME(dw, soc.CDim_DataDostawy) WHEN 'Niedziela' THEN 1 ELSE NULL END as 'Sunday Delivery (SUN)'"
 + ", NULL as 'Delivery on Holidays (HOLI)'"
 + ", NULL as 'TRSP'"
 + ", NULL as 'FULL'"
 + ", NULL as 'Phone no. (HMD)'"
 + ", NULL as 'Time window (HMD)'"
 + ", NULL as 'Advice of Delivery (ADV)'"
 + ", NULL as 'Cash On Delivery value (COD)'"
 + ", NULL as 'Cash for transport (CFT)'"
 + ", NULL as 'E-mail cosignee notification (EML)'"
 + ", NULL as 'E-mail notification address (EML)'"
 + ", NULL as 'Hand loading (HLO)'"
 + ", NULL as 'Hand unloading (HUN)'"
 + ", NULL as 'Text message notification (ISMS)'"
 + ", NULL as 'Mobile phone no. (iSMS)'"
 + ", NULL as 'Product line'"
 + ", NULL as 'Cosignor private individual person'"
 + ", NULL as 'Loading place private individual person'"
 + ", NULL as 'Consignee private individual person'"
 + ", NULL as 'Unloading place private individual person'"
 + ", NULL as 'Additinoal reference on invoice'"
 + ", NULL as 'Incoterms'"
 + ", NULL as 'EKAER no. / HU only'"
 + ", NULL as 'Goods monitored (SENT) / PL only'"
 + ", NULL as 'Weight LQ'"
 + ", NULL as 'Financial no. COD / UA only'"
 + ", NULL as 'ADR line ID'"
 + ", NULL as 'ADR UN no.'"
 + ", NULL as 'ADR name'"
 + ", NULL as 'ADR pack group'"
 + ", NULL as 'ADR technical name'"
 + ", NULL as 'ADR quantity'"
 + ", NULL as 'ADR type of packaging'"
 + ", NULL as 'ADR net weight'"
 + ", NULL as 'ADR volume'"
 + ", NULL as 'ADR hazardous'"
 + ", NULL as 'Incoterms city'"
 + ", NULL as 'Incoterms Country'"
 + ", NULL as 'Raben Time Slots (RTS)'"
 + ", NULL as 'Lift (LTD)'"
 + ", NULL as 'UNLO'"
 + ", NULL as 'HAND'"
 + ", NULL as 'SP11'"
 + ", NULL as 'Shipment Transfer on Saturday (TRSO)'"
 + ", NULL as 'Shipment transfer on Sunday (TRND)'"
 + ", NULL as 'Call before delivery (CALL)'"
 + ", NULL as 'Advice of Pick-up (ADVPU)'"
 + ", NULL as 'Exact delivery on specific hour (EXL)'"
 + ", NULL as 'Return of empty packages (PEP)'"
 + ", NULL as 'Own Time Slots (OTS)'"
 + ", NULL as 'Delivery Distribution Center (DDC)'"
 + ", NULL as 'Cargo Article Sorting (CAS)'"
 + ", NULL as 'Next day delivery 08 (ND08)'"
 + ", NULL as 'Next day delivery 10 (ND10)'"
 + ", NULL as 'Next day delivery  12 (ND12)'"
 + ", NULL as 'Next day delivery 16 (ND16)'"
 + ", NULL as 'Fix day delivery (FIX)'"
 + ", NULL as 'Fix day delivery 08 (FIX08)'"
 + ", NULL as 'Fix day delivery 10 (FIX10)'"
 + ", NULL as 'Fix day delivery 12 (FIX12)'"
 + ", NULL as 'My delivery (MYD)'"
 + ", NULL as 'Scaning During Delivery (SDD)'"
 + ", NULL as 'Picture During Delivery (PDD)'"
 + ", NULL as 'Picture Confirming Delivery (PCD)'"
 + ", NULL as 'Hiding the address (NES) - SN'"
 + ", NULL as 'Truck with lift (LIFT_FL) / only Fresh'"
 + ", NULL as 'Sing on glass (SOD)'"
 + ", NULL as 'man 2 handling (2MAN)'"
 + ", NULL as 'Evening delivery (EVE)'"
 + ", NULL as 'Collection day guarantee (CDG)'"
 + ", NULL as 'JiT (JMP) Wave 1 (FAL1)'"
 + ", NULL as 'JiT (JMP) Wave 2 (FAL2)'"
 + ", NULL as 'Additional Pellets - quantity (ADP)'"
 + ", NULL as 'Transport unit type (ADP)'"
 + ", NULL as 'MyDelivery plus (MYDP)'"
 + ", NULL as 'TLD'"
 + ", NULL as 'BIO products weight'"
 + ", NULL as 'Shipment Transfer on Monday-Friday (TRBP)'"
 + ", NULL as 'Shipment Transfer on Saturday (TRBS)'"
 + ", NULL as 'Shipment Transfer on Sunday (TRBN)'"
 + ", NULL as 'Time window (TRxx) HH:MM - HH:MM'"
 + ", NULL as 'ADR technical name (English)'"
 + ", NULL as 'Tail Lift Pickup (TLP)'"
 + ", NULL as 'Intermodal (IMDL)'"
 + ", NULL as 'WD'"
 + ", NULL as 'Loading reference'"
 + ", NULL as 'Unloading reference'"
 + " from HM.ZO zo left join SSCommon.vKontrahenci nab on nab.id = zo.khid " +
 "left join SSCommon.vKontrahenci odb on odb.id = zo.odid " +
 "left join SSCommon.HMF_SalesOrderClassification soc on soc.ElementId = zo.id " +
 "left join SSCommon.HMF_SalesOrderClassification wdc on wdc.ElementId = zo.id " +
 "where (zo.data = '" + data + "') and zo.typ_dk like '%ZO%'";
            return query;
        }
    }
}
