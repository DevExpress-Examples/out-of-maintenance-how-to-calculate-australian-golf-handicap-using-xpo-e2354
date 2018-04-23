Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB

Public Class Result
    Inherits XPObject
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
    End Sub

    Protected _Date As DateTime
    Public Property [Date]() As DateTime
        Get
            Return _Date
        End Get
        Set(ByVal value As DateTime)
            SetPropertyValue(Of DateTime)("Date", _Date, value)
        End Set
    End Property

    Protected _CourseRating As Int32
    Public Property CourseRating() As Int32
        Get
            Return _CourseRating
        End Get
        Set(ByVal value As Int32)
            SetPropertyValue(Of Int32)("CourseRating", _CourseRating, value)
        End Set
    End Property

    Protected _Gross As Int32
    Public Property Gross() As Int32
        Get
            Return _Gross
        End Get
        Set(ByVal value As Int32)
            SetPropertyValue(Of Int32)("Gross", _Gross, value)
        End Set
    End Property

    <PersistentAlias("Gross - CourseRating")> _
    Public ReadOnly Property PlayedTo() As Int32
        Get
            Return Convert.ToInt32(EvaluateAlias("PlayedTo"))
        End Get
    End Property

    Protected _Player As Player
    <Association("Player-Results", GetType(Player))> _
    Public Property Player() As Player
        Get
            Return _Player
        End Get
        Set(ByVal value As Player)
            SetPropertyValue(Of Player)("Player", _Player, value)
        End Set
    End Property
End Class
