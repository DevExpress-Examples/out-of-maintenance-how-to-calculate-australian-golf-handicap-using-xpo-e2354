using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

/// <summary>
/// Summary description for XpoHelper
/// </summary>
public static class XpoHelper {
    static XpoHelper() {
        CreateDefaultObjects();
    }

    public static Session GetNewSession() {
        return new Session(DataLayer);
    }

    public static UnitOfWork GetNewUnitOfWork() {
        return new UnitOfWork(DataLayer);
    }

    private readonly static object lockObject = new object();

    static IDataLayer fDataLayer;
    static IDataLayer DataLayer {
        get {
            if (fDataLayer == null) {
                lock (lockObject) {
                    fDataLayer = GetDataLayer();
                }
            }
            return fDataLayer;
        }
    }

    private static IDataLayer GetDataLayer() {
        XpoDefault.Session = null;

        InMemoryDataStore ds = new InMemoryDataStore();
        XPDictionary dict = new ReflectionDictionary();
        dict.GetDataStoreSchema(typeof(Player).Assembly);
        dict.GetDataStoreSchema(typeof(Result).Assembly);

        return new ThreadSafeDataLayer(dict, ds);
    }

    static void CreateDefaultObjects() {
        using (UnitOfWork uow = GetNewUnitOfWork()) {
            Player player = new Player(uow);
            
            player.Name = "Vest";
            player.Save();

            Int32[] GrossScoreVest = { 87, 87, 94, 89, 92, 82, 85, 93, 90, 89, 83, 88, 94, 90, 83, 85, 88, 83, 90, 89 };
            const Int32 CourseRating = 70;

            DateTime date = DateTime.Now;

            foreach (Int32 gs in GrossScoreVest)
            {
                Result result = new Result(uow);
                
                result.Player = player;
                result.Date = date;
                result.Gross = gs;
                result.CourseRating = CourseRating;

                result.Save();

                date = date.AddDays(1.0);
            }

            player = new Player(uow);

            player.Name = "Serge";
            player.Save();

            Int32[] GrossScoreSerge = { 88, 85, 95, 83, 90, 81, 85, 95, 95, 87, 87, 88, 96, 90, 87, 82, 85, 81, 91, 87 };

            date = DateTime.Now;
            date.AddHours(1.0);

            foreach (Int32 gs in GrossScoreSerge) {
                Result result = new Result(uow);

                result.Player = player;
                result.Date = date;
                result.Gross = gs;
                result.CourseRating = CourseRating;

                result.Save();

                date = date.AddDays(1.0);
            }
            
            uow.CommitChanges();
        }
    }
}