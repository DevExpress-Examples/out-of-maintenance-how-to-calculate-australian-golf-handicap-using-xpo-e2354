using System;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;

public class Result : XPObject {
    public Result()
        : base() { }

    public Result(Session session)
        : base(session) { }

    public override void AfterConstruction() {
        base.AfterConstruction();        
    }

    protected DateTime _Date;
    public DateTime Date {
        get { return _Date; }
        set { SetPropertyValue<DateTime>("Date", ref _Date, value); }
    }

    protected Int32 _CourseRating;
    public Int32 CourseRating {
        get { return _CourseRating; }
        set { SetPropertyValue<Int32>("CourseRating", ref _CourseRating, value); }
    }

    protected Int32 _Gross;
    public Int32 Gross {
        get { return _Gross; }
        set { SetPropertyValue<Int32>("Gross", ref _Gross, value); }
    }

    [PersistentAlias("Gross - CourseRating")]
    public Int32 PlayedTo {
        get { return Convert.ToInt32(EvaluateAlias("PlayedTo")); }
    }

    protected Player _Player;
    [Association("Player-Results", typeof(Player))]
    public Player Player {
        get { return _Player; }
        set { SetPropertyValue<Player>("Player", ref _Player, value); }
    }
}
