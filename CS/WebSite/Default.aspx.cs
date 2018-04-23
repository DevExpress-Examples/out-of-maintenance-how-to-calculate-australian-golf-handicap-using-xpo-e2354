using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Web.ASPxGridView;
using System.Collections.Generic;
using System.Drawing;

public partial class _Default : System.Web.UI.Page {
    Session session = XpoHelper.GetNewSession();

    protected void Page_Init(object sender, EventArgs e) {
        xdsPlayers.Session = session;
        xdsResults.Session = session;
    }

    protected void gridResults_BeforePerformDataSelect(object sender, EventArgs e) {
        ASPxGridView grid = sender as ASPxGridView;
        GridViewPreviewRowTemplateContainer container = grid.NamingContainer as GridViewPreviewRowTemplateContainer;

        xdsResults.Criteria = String.Format("[Player!Key] = {0}", container.KeyValue);
    }

    List<Result> lastBestTen;
    protected void gridResults_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e) {
        ASPxGridView grid = sender as ASPxGridView;
        GridViewPreviewRowTemplateContainer container = grid.NamingContainer as GridViewPreviewRowTemplateContainer;

        Player player = (Player)container.Grid.GetRow(container.VisibleIndex);

        if (lastBestTen == null)
            lastBestTen = player.LastBestTen();

        if (lastBestTen.Count == 0)
            return;

        if (lastBestTen[0].Player != player)
            lastBestTen = player.LastBestTen();

        if (lastBestTen.Find(result => result.Oid == Convert.ToInt32(e.KeyValue)) != null)
            e.Row.BackColor = Color.Yellow;
    }

    protected void gridResults_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e) {
        ASPxGridView grid = sender as ASPxGridView;
        GridViewPreviewRowTemplateContainer container = grid.NamingContainer as GridViewPreviewRowTemplateContainer;

        e.NewValues["Date"] = DateTime.Now;
        e.NewValues["CourseRating"] = 70;
    }

    protected void gridResults_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e) {
        ASPxGridView grid = sender as ASPxGridView;
        GridViewPreviewRowTemplateContainer container = grid.NamingContainer as GridViewPreviewRowTemplateContainer;

        e.NewValues["Player!Key"] = container.KeyValue;

    }

    protected void gridResults_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e) {
        ASPxGridView grid = sender as ASPxGridView;
        grid.JSProperties["cpRefresh"] = true;
    }

    protected void gridResults_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e) {
        ASPxGridView grid = sender as ASPxGridView;
        grid.JSProperties["cpRefresh"] = true;
    }
}
