<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Xpo.v15.1, Version=15.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Xpo" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>How to calculate Australian Golf Handicap using XPO</title>

	<script type="text/javascript">
		function OnResultsEndCallback(s, e) {
			if (s.cpRefresh) {
				delete s.cpRefresh;
				gridPlayers.Refresh();
			}
		}
	</script>

</head>
<body>
	<form id="form1" runat="server">
	<div>
		<dx:ASPxGridView ID="gridPlayers" runat="server" AutoGenerateColumns="False" DataSourceID="xdsPlayers"
			ClientInstanceName="gridPlayers" KeyFieldName="Oid">
			<Templates>
				<PreviewRow>
					<dx:ASPxGridView ID="gridResults" runat="server" Width="100%" AutoGenerateColumns="False"
						DataSourceID="xdsResults" KeyFieldName="Oid" OnBeforePerformDataSelect="gridResults_BeforePerformDataSelect"
						EnableRowsCache="false" OnHtmlRowPrepared="gridResults_HtmlRowPrepared" OnInitNewRow="gridResults_InitNewRow"
						OnRowInserting="gridResults_RowInserting" OnRowInserted="gridResults_RowInserted"
						OnRowUpdated="gridResults_RowUpdated">
						<Columns>
                            <dx:GridViewCommandColumn VisibleIndex="0" ShowNewButton="True" ShowEditButton="True"/>
							<dx:GridViewDataTextColumn FieldName="Oid" ReadOnly="True" VisibleIndex="0" Visible="false">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataDateColumn FieldName="Date" VisibleIndex="0" SortIndex="0" SortOrder="Descending">
							</dx:GridViewDataDateColumn>
							<dx:GridViewDataSpinEditColumn FieldName="Gross" VisibleIndex="1">
							</dx:GridViewDataSpinEditColumn>
							<dx:GridViewDataSpinEditColumn FieldName="CourseRating" VisibleIndex="2">
							</dx:GridViewDataSpinEditColumn>
							<dx:GridViewDataTextColumn FieldName="PlayedTo" ReadOnly="True" VisibleIndex="3">
								<EditFormSettings Visible="False" />
							</dx:GridViewDataTextColumn>
						</Columns>
						<ClientSideEvents EndCallback="OnResultsEndCallback" />
					</dx:ASPxGridView>
				</PreviewRow>
				<TitlePanel>
					<i>Your exact handicap will be the average of the best 10 differentials<br />
						(differential = gross score - AMCR/AWCR) of your 20 most recent
						<br />
						valid scores, the result of which is multiplied by 0.96.</i></TitlePanel>
			</Templates>
			<Settings ShowPreview="True" ShowTitlePanel="true" />
			<SettingsPager PageSize="1">
			</SettingsPager>
			<Columns>
                <dx:GridViewCommandColumn VisibleIndex="0" ShowNewButton="True" ShowDeleteButton="True"/>
				<dx:GridViewDataTextColumn FieldName="Oid" ReadOnly="True" VisibleIndex="0" SortIndex="0"
					SortOrder="Ascending" Visible="false">
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="1">
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="Handicap" ReadOnly="True" VisibleIndex="2">
					<CellStyle Font-Bold="True">
					</CellStyle>
					<EditFormSettings Visible="False" />
				</dx:GridViewDataTextColumn>
			</Columns>
		</dx:ASPxGridView>
		<dx:XpoDataSource ID="xdsPlayers" runat="server" TypeName="Player" ServerMode="true">
		</dx:XpoDataSource>
		<dx:XpoDataSource ID="xdsResults" runat="server" TypeName="Result" ServerMode="true">
		</dx:XpoDataSource>
	</div>
	</form>
</body>
</html>
