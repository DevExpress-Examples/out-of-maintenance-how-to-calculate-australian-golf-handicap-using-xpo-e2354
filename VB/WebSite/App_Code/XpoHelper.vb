Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports DevExpress.Xpo.Metadata

''' <summary>
''' Summary description for XpoHelper
''' </summary>
Public NotInheritable Class XpoHelper
	Private Sub New()
	End Sub
	Shared Sub New()
		CreateDefaultObjects()
	End Sub

	Public Shared Function GetNewSession() As Session
		Return New Session(DataLayer)
	End Function

	Public Shared Function GetNewUnitOfWork() As UnitOfWork
		Return New UnitOfWork(DataLayer)
	End Function

	Private ReadOnly Shared lockObject As Object = New Object()

	Private Shared fDataLayer As IDataLayer
	Private Shared ReadOnly Property DataLayer() As IDataLayer
		Get
			If fDataLayer Is Nothing Then
				SyncLock lockObject
					fDataLayer = GetDataLayer()
				End SyncLock
			End If
			Return fDataLayer
		End Get
	End Property

	Private Shared Function GetDataLayer() As IDataLayer
		XpoDefault.Session = Nothing

		Dim ds As New InMemoryDataStore()
		Dim dict As XPDictionary = New ReflectionDictionary()
		dict.GetDataStoreSchema(GetType(Player).Assembly)
		dict.GetDataStoreSchema(GetType(Result).Assembly)

		Return New ThreadSafeDataLayer(dict, ds)
	End Function

	Private Shared Sub CreateDefaultObjects()
		Using uow As UnitOfWork = GetNewUnitOfWork()
			Dim player As New Player(uow)

			player.Name = "Vest"
			player.Save()

			Dim GrossScoreVest() As Int32 = { 87, 87, 94, 89, 92, 82, 85, 93, 90, 89, 83, 88, 94, 90, 83, 85, 88, 83, 90, 89 }
			Const CourseRating As Int32 = 70

			Dim [date] As DateTime = DateTime.Now

			For Each gs As Int32 In GrossScoreVest
				Dim result As New Result(uow)

				result.Player = player
				result.Date = [date]
				result.Gross = gs
				result.CourseRating = CourseRating

				result.Save()

				[date] = [date].AddDays(1.0)
			Next gs

			player = New Player(uow)

			player.Name = "Serge"
			player.Save()

			Dim GrossScoreSerge() As Int32 = { 88, 85, 95, 83, 90, 81, 85, 95, 95, 87, 87, 88, 96, 90, 87, 82, 85, 81, 91, 87 }

			[date] = DateTime.Now
			[date].AddHours(1.0)

			For Each gs As Int32 In GrossScoreSerge
				Dim result As New Result(uow)

				result.Player = player
				result.Date = [date]
				result.Gross = gs
				result.CourseRating = CourseRating

				result.Save()

				[date] = [date].AddDays(1.0)
			Next gs

			uow.CommitChanges()
        End Using
	End Sub
End Class