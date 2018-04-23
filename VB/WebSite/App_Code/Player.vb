Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports System.Collections.Generic

Public Class Player
	Inherits XPObject
	Private Const BonusForExcellense As Double = 0.96

	Public Sub New()
		MyBase.New()
	End Sub

	Public Sub New(ByVal session As Session)
		MyBase.New(session)
	End Sub

	Public Overrides Sub AfterConstruction()
		MyBase.AfterConstruction()
	End Sub

	Protected _Name As String
	Public Property Name() As String
		Get
			Return _Name
		End Get
		Set(ByVal value As String)
			SetPropertyValue(Of String)("Name", _Name, value)
		End Set
	End Property

	<NonPersistent> _
	Public ReadOnly Property Handicap() As Int32
		Get
			Dim results As List(Of Result) = LastBestTen()

			If results.Count = 0 Then
				Return 0
			End If

            Dim hc As Double = 0.0

            hc = results.Average(Function(x) x.PlayedTo) ' average

            hc *= BonusForExcellense ' avarage * 0.96
            hc = Math.Round(Math.Truncate(hc * 10) / 10) ' 14.496 -> 14.4 -> 14

            Return Convert.ToInt32(hc)
		End Get
	End Property

	<Association("Player-Results", GetType(Result))> _
	Public ReadOnly Property Results() As XPCollection(Of Result)
		Get
			Return GetCollection(Of Result)("Results")
		End Get
	End Property

	Public Function LastBestTen() As List(Of Result)
        If Me.Results.Count <= 10 Then
            Return Me.Results.ToList()
        End If

		Dim results As XPQuery(Of Result) = New XPQuery(Of Result)(Me.Session)

        Dim list1 = (From r In results _
                     Where (r.Player Is Me) _
                     Order By r.Date Descending _
                     Select r).Take(20).ToList()

		Dim list2 = (From r In list1 _
		             Order By r.PlayedTo Ascending _
		             Select r).Take(10)

        Return list2.ToList()
	End Function
End Class

