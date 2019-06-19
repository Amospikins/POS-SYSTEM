Imports System.Drawing
Imports Microsoft.VisualBasic.PowerPacks.Printing.Compatibility.VB6
Public Class PrinterClass
    Private p As Printer
    Private _Path As String
    Private _Align As TextAlignment = TextAlignment.Default
    Private bIsDebug As Boolean = True
    Public Enum TextAlignment As Byte
        [Default] = 0
        Left
        Center
        Right
    End Enum
#Region "Constructors"
    Public Sub New(ByVal AppPath As String)
        SetPrinterName("POS58 10.0.0.6", AppPath)
    End Sub
    Public Sub New(ByVal strPrinterName As String, ByVal AppPath As String)
        SetPrinterName(strPrinterName, AppPath)
    End Sub
    Private Sub SetPrinterName(ByVal PrinterName As String, ByVal AppPath As String)
        Dim prnPrinter As Printer

        For Each prnPrinter In Printers
            If prnPrinter.DeviceName = PrinterName Then
                p = prnPrinter
                Exit For
            End If
        Next
        p.DocumentName = "ERP System"
        Me.Path = AppPath
        If bIsDebug Then
            p.PrintAction = Printing.PrintAction.PrintToPreview
        End If
    End Sub
#End Region
#Region "Images"
    Public Property Path() As String
        Get
            Return _Path
        End Get
        Set(ByVal value As String)
            _Path = value
        End Set
    End Property
    Public Sub PrintLogo()
        Me.PrintImage(_Path & "\hexx.png")
        p.CurrentY += 500 + 100
    End Sub
    Private Sub PrintImage(ByVal FileName As String)
        Dim pic As Image

        pic = pic.FromFile(FileName)

        p.PaintPicture(pic, p.CurrentX, p.CurrentY)
        p.CurrentY = p.CurrentY + pic.Height
    End Sub
#End Region
#Region "Font"

    Public Property Alignment() As TextAlignment
        Get
            Return _Align
        End Get
        Set(ByVal value As TextAlignment)
            _Align = value
        End Set
    End Property
    Public Sub AlignLeft()
        _Align = TextAlignment.Left
    End Sub
    Public Sub AlignCenter()
        _Align = TextAlignment.Center
    End Sub
    Public Sub AlignRight()
        _Align = TextAlignment.Right
    End Sub
    Public Property FontName() As String
        Get
            Return p.FontName
        End Get
        Set(ByVal value As String)
            p.FontName = value
        End Set
    End Property

    Public Property FontSize() As Single
        Get
            Return p.FontSize
        End Get
        Set(ByVal value As Single)
            p.FontSize = value
        End Set
    End Property
    Public Property Bold() As Boolean
        Get
            Return p.FontBold
        End Get
        Set(ByVal value As Boolean)
            p.FontBold = value
        End Set
    End Property
    Public Sub DrawLine()
        p.DrawWidth = 2
        p.Line(p.Width, p.CurrentY)
        p.CurrentY += 20
    End Sub
    Public Sub NormalFont()
        Me.FontSize = 9.5F
    End Sub
    Public Sub BigFont()
        Me.FontSize = 15.0F
    End Sub
    Public Sub SmallFont()
        Me.FontSize = 6.0F
    End Sub

    Public Sub SetFont(Optional ByVal FontSize As Single = 9.5F, Optional ByVal FontName As String = "FontA1x1", Optional ByVal BoldType As Boolean = False)
        Me.FontSize = FontSize
        Me.FontName = FontName
        Me.Bold = BoldType
    End Sub
#End Region

#Region "Control"

    Public Sub NewPage()
        p.NewPage()


    End Sub
    Public Property RTL() As Boolean
        Get
            Return p.RightToLeft
        End Get
        Set(ByVal value As Boolean)
            p.RightToLeft = value
        End Set
    End Property
    Public Sub FeedPaper(Optional ByVal nlines As Integer = 3)
        For i As Integer = 1 To nlines
            Me.WriteLine("")
        Next
    End Sub

    Public Sub GotoCol(Optional ByVal ColNumber As Integer = 0)
        Dim ColWidth As Single = p.Width / 48
        p.CurrentX = ColWidth * ColNumber
    End Sub
    Public Sub GotoSixth(Optional ByVal nSixth As Integer = 1)
        Dim OneSixth As Single = p.Width / 6
        p.CurrentX = OneSixth * (nSixth - 1)
    End Sub


    Public Sub UnderlineOn()
        p.FontUnderline = True
    End Sub
    Public Sub UnderlineOff()
        p.FontUnderline = False
    End Sub
    Public Sub EndDoc()
        p.EndDoc()
    End Sub
    Public Sub EndJob()
        Me.EndDoc()
    End Sub
    Public Sub WriteLine(ByVal Text As String)
        Dim sTextWidth As Single = p.TextWidth(Text)
        Select Case _Align
            Case TextAlignment.Default
                'do nothing
            Case TextAlignment.Left
                p.CurrentX = 0
            Case TextAlignment.Center
                p.CurrentX = (p.Width - sTextWidth) / 2
            Case TextAlignment.Right
                p.CurrentX = (p.Width - sTextWidth)
        End Select
        p.Print(Text)
    End Sub
    Public Sub WriteChars(ByVal Text As String)
        p.Write(Text)
    End Sub
    Public Sub CutPaper()
        p.NewPage()
    End Sub

#End Region


End Class
