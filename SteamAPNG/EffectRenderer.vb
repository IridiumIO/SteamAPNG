Imports System.ComponentModel
Imports System.Windows.Threading

Public Class EffectRenderer : Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Dim RenderTimer As New DispatcherTimer(DispatcherPriority.Send)

    Public Property AnimationFrames As List(Of Byte())
    Public Property FPS = 30
    Public Property FrameIndex = 0
    Public Property CurrentFrame As BitmapImage = Nothing

    Sub New()
        AddHandler RenderTimer.Tick, AddressOf ElapsedT
        RenderTimer.Interval = TimeSpan.FromMilliseconds(1000 / FPS)
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

    Public Sub SetFPS(inFPS As Integer)
        RenderTimer.Interval = TimeSpan.FromMilliseconds(1000 / inFPS)
    End Sub

End Class
