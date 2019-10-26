Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Threading

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
        FrameIndex = 0
        AnimationFrames = ReadZipToBytes(effectPath.FullName)
        RenderTimer.Start()
    End Sub


    Public Property TF_TranslationX As Double = 0
    Public Property TF_TranslationY As Double = 0

    Public Property TF_Rotation As Double = 0
    Public Property TF_Opacity As Double = 1

    Public Property TF_ScaleX As Double = 1
    Public Property TF_ScaleY As Double = 1


    Public Property StatusString = "Idle"

    Public Sub ResetTransforms()
        TF_TranslationX = 0
        TF_TranslationY = 0
        TF_Rotation = 0
        TF_Opacity = 1
        TF_ScaleX = 1
        TF_ScaleY = 1
    End Sub

    Public Function Merge(bgImg, overlayFrame, counter) As Byte()
        Dim tra As New TranslateTransform(TF_TranslationX, TF_TranslationY)
        Dim rot As New RotateTransform(TF_Rotation, overlayFrame.Width / 2, overlayFrame.Height / 2)
        Dim sct As New ScaleTransform(TF_ScaleX, TF_ScaleY, overlayFrame.Width / 2, overlayFrame.Height / 2)

        Dim visual As New DrawingVisual

        Using ctx = visual.RenderOpen
            ctx.DrawImage(bgImg, New Rect(New Size(600, 900)))

            ctx.PushOpacity(TF_Opacity)
            ctx.PushTransform(tra)
            ctx.PushTransform(rot)
            ctx.PushTransform(sct)
            ctx.DrawImage(overlayFrame, New Rect(0, 0, overlayFrame.Width, overlayFrame.Height))
            ctx.Pop()
            ctx.Pop()
            ctx.Pop()
            ctx.Pop()

        End Using

        Dim bitmap As RenderTargetBitmap = New RenderTargetBitmap(600, 900, 96, 96, PixelFormats.Pbgra32)
        bitmap.Render(visual)
        bitmap.Freeze()

        Dim pngRend As New PngBitmapEncoder
        pngRend.Frames.Add(BitmapFrame.Create(bitmap))
        Using ms As New MemoryStream()
            pngRend.Save(ms)
            StatusString = $"Generating Frames...{counter}/{AnimationFrames.Count}"
            Return ms.ToArray()
        End Using

    End Function

    Public Function WriteImages(baseImage As BitmapImage) As List(Of Byte())
        Dim combinedImages As New List(Of Byte())
        Dim i As Integer = 0
        For Each img In AnimationFrames
            Dim CombImg = Application.Current.Dispatcher.Invoke(Function() Merge(baseImage, ImageFromBytes(img), i))

            combinedImages.Add(CombImg)
            i += 1
        Next

        Return combinedImages

    End Function

End Class

