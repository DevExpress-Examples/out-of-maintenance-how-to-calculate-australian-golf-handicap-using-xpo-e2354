using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;

public class Player : XPObject {
    const Double BonusForExcellense = 0.96;

    public Player()
        : base() { }

    public Player(Session session)
        : base(session) { }

    public override void AfterConstruction() {
        base.AfterConstruction();
    }

    protected String _Name;
    public String Name {
        get { return _Name; }
        set { SetPropertyValue<String>("Name", ref _Name, value); }
    }

    [NonPersistent]
    public Int32 Handicap {
        get {
            List<Result> results = LastBestTen();

            if (results.Count == 0)
                return 0;

            Double handicap = 0.0;

            handicap = results.Average<Result>(x => x.PlayedTo); // average
       
            handicap *= BonusForExcellense; // avarage * 0.96
            handicap = Math.Round(Math.Truncate(handicap * 10) / 10); // 14.496 -> 14.4 -> 14

            return Convert.ToInt32(handicap);
        }
    }

    [Association("Player-Results", typeof(Result))]
    public XPCollection<Result> Results {
        get { return GetCollection<Result>("Results"); }
    }

    public List<Result> LastBestTen() {
        if (Results.Count <= 10)
            return Results.ToList<Result>();

        XPQuery<Result> results = new XPQuery<Result>(this.Session);

        var list1 = (from r in results
                     where (r.Player == this)
                     orderby r.Date descending
                     select r).Take(20).ToList<Result>();

        var list2 = (from r in list1
                     orderby r.PlayedTo ascending
                     select r).Take(10);

        return list2.ToList<Result>();
    }
}

