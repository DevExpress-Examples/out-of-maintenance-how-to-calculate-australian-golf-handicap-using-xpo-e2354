Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.Web.ASPxGridView
Imports System.Collections.Generic
Imports System.Drawing

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Private session As Session = XpoHelper.GetNewSession()

	Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
		xdsPlayers.Session = session
		xdsResults.Session = session
	End Sub

	Protected Sub gridResults_BeforePerformDataSelect(ByVal sender As Object, ByVal e As EventArgs)
		Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)
		Dim container As GridViewPreviewRowTemplateContainer = TryCast(grid.NamingContainer, GridViewPreviewRowTemplateContainer)

		xdsResults.Criteria = String.Format("[Player!Key] = {0}", container.KeyValue)
	End Sub

	Private lastBestTen As List(Of Result)
	Protected Sub gridResults_HtmlRowPrepared(ByVal sender As Object, ByVal e As ASPxGridViewTableRowEventArgs)
		Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)
		Dim container As GridViewPreviewRowTemplateContainer = TryCast(grid.NamingContainer, GridViewPreviewRowTemplateContainer)

		Dim player As Player = CType(container.Grid.GetRow(container.VisibleIndex), Player)

		If lastBestTen Is Nothing Then
			lastBestTen = player.LastBestTen()
		End If

		If lastBestTen.Count = 0 Then
			Return
		End If

		If lastBestTen(0).Player IsNot player Then
			lastBestTen = player.LastBestTen()
		End If

		If lastBestTen.Find(Function(result) result.Oid = Convert.ToInt32(e.KeyValue)) IsNot Nothing Then
			e.Row.BackColor = Color.Yellow
		End If
	End Sub

	Protected Sub gridResults_InitNewRow(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInitNewRowEventArgs)
		Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)
		Dim container As GridViewPreviewRowTemplateContainer = TryCast(grid.NamingContainer, GridViewPreviewRowTemplateContainer)

		e.NewValues("Date") = DateTime.Now
		e.NewValues("CourseRating") = 70
	End Sub

	Protected Sub gridResults_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs)
		Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)
		Dim container As GridViewPreviewRowTemplateContainer = TryCast(grid.NamingContainer, GridViewPreviewRowTemplateContainer)

		e.NewValues("Player!Key") = container.KeyValue

	End Sub

	Protected Sub gridResults_RowUpdated(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatedEventArgs)
		Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)
		grid.JSProperties("cpRefresh") = True
	End Sub

	Protected Sub gridResults_RowInserted(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertedEventArgs)
		Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)
		grid.JSProperties("cpRefresh") = True
	End Sub
End Class
