Imports System.ComponentModel

Public Class EffectRenderer : Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    ReadOnly RenderTimer As New Timers.Timer

    Public Property AnimationFrames As List(Of Byte())

    Public Property FrameIndex = 0
    Public Property CurrentFrame As BitmapImage

    Private _FPS = 30
    Public Property FPS
        Get
            Return _FPS
        End Get
        Set(value)
            _FPS = value
            RenderTimer.Interval = 1000 / value
        End Set
    End Property

    Sub New()
        AddHandler RenderTimer.Elapsed, AddressOf ElapsedT
        RenderTimer.Interval = 1000 / FPS
    End Sub

    Private Sub ElapsedT()
        If FrameIndex = AnimationFrames.Count Then FrameIndex = 0
        CurrentFrame = ImageFromBytes(AnimationFrames(FrameIndex))
        FrameIndex += 1
    End Sub

    Public Sub LoadEffectSequence(effectPath As IO.FileInfo)
        RenderTimer.Stop()
        FrameIndex = 0
        AnimationFrames = ReadZipToBytes(effectPath.FullName)
        RenderTimer.Start()
    End Sub


End Class

