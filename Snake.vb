Public Class Snake
    Enum Directions
        UP
        DOWN
        LEFT
        RIGHT
    End Enum

    Enum GameStates
        PREGAME
        START
        RUN
        STOPGAME
    End Enum

    Dim snakeHead As Point = New Point(8, 8)
    Dim currentDirection As Int16
    Dim snakeBody As List(Of Point) = New List(Of Point)
    Dim snakeLength As Int16 = 4
    Dim cherry As Point
    Dim gameStop As Boolean = False
    Dim gameState As Int16 = GameStates.PREGAME
    Const boardSize As Int16 = 16
    Dim Corner As Point = New Point(0, 0)

    Sub PreGame()
        Static flipFlop As Boolean
        Dim g As Graphics = PictureBox1.CreateGraphics
        Dim startimage As Image = Image.FromFile("C:\Users\callk\source\repos\Snake\SnakeStartScreen.png")

        g.DrawImage(startimage, Corner)

        If flipFlop = True Then
            g.DrawString("Press Enter to Start", New Font("Arial", 16), New SolidBrush(Color.Black), New Point(80, 300))
            flipFlop = False
            Return
        End If
        flipFlop = True
        'StartLabel.Visible = Not StartLabel.Visible
        g.Dispose()
    End Sub

    Sub StartGame()
        Randomize()
        Dim g As Graphics = PictureBox1.CreateGraphics

        g.Clear(PictureBox1.BackColor)
        StartLabel.Visible = False
        GameBoard()
        '
        gameStop = False
        snakeHead = New Point(8, 8)
        snakeLength = 4
        cherry = New Point(Rnd(1) * boardSize, Rnd(1) * boardSize)
        g.FillRectangle(New SolidBrush(Color.OrangeRed), CInt(PictureBox1.Width * (cherry.X / boardSize)), CInt(PictureBox1.Height * (cherry.Y / boardSize)), CInt(PictureBox1.Width / boardSize), CInt(PictureBox1.Height / boardSize))
        gameState = GameStates.RUN
        g.Dispose()
    End Sub

    Sub snakeDraw()
        Dim g As Graphics = PictureBox1.CreateGraphics
        If gameStop = False Then
            Select Case currentDirection
                Case Directions.UP
                    snakeHead = New Point(snakeHead.X, snakeHead.Y - 1)
                Case Directions.DOWN
                    snakeHead = New Point(snakeHead.X, snakeHead.Y + 1)
                Case Directions.LEFT
                    snakeHead = New Point(snakeHead.X - 1, snakeHead.Y)
                Case Directions.RIGHT
                    snakeHead = New Point(snakeHead.X + 1, snakeHead.Y)
            End Select
            g.FillRectangle(Brushes.Blue, CInt(PictureBox1.Width * (snakeHead.X / boardSize) + 1), CInt(PictureBox1.Height * (snakeHead.Y / boardSize) + 1), CInt(PictureBox1.Width / boardSize) - 1, CInt(PictureBox1.Height / boardSize) - 1)

            snakeBody.Insert(0, snakeHead)

            If snakeBody.Count > snakeLength Then
                'Erase thing
                Dim rattle As Point = snakeBody.Last()
                g.FillRectangle(New SolidBrush(Me.BackColor), CInt(PictureBox1.Width * (rattle.X / boardSize) + 1), CInt(PictureBox1.Height * (rattle.Y / boardSize) + 1), CInt(PictureBox1.Width / boardSize - 1), CInt(PictureBox1.Height / boardSize) - 1)

                snakeBody.Remove(rattle)
            End If
        End If
        Collision()
        g.Dispose()
    End Sub

    Sub Collision()
        Dim g As Graphics = PictureBox1.CreateGraphics
        'Handles Cherry Collision
        If snakeHead = cherry Then
            snakeLength += 1
            cherry = New Point(Rnd(1) * boardSize, Rnd(1) * boardSize)
        End If
        g.FillRectangle(New SolidBrush(Color.OrangeRed), CInt(PictureBox1.Width * (cherry.X / boardSize) + 1), CInt(PictureBox1.Height * (cherry.Y / boardSize) + 1), CInt(PictureBox1.Width / boardSize) - 1, CInt(PictureBox1.Height / boardSize) - 1)

        'Handles Wall Collision
        If boardSize - 1 < snakeHead.X Or boardSize - 1 < snakeHead.Y Or 0 > snakeHead.X Or 0 > snakeHead.Y Then
            gameStop = True
            gameState = GameStates.STOPGAME
        End If

        'Handles Body Collision
        For i = 1 To snakeBody.Count - 1
            If snakeHead = snakeBody.Item(i) Then
                gameStop = True
                gameState = GameStates.STOPGAME
            End If
        Next

        g.Dispose()
    End Sub

    Sub GameBoard()
        Dim g As Graphics = PictureBox1.CreateGraphics
        Dim boardPen As Pen = New Pen(Color.Black)

        For i = 0 To boardSize
            g.DrawLine(boardPen, CSng(PictureBox1.Width * (i / boardSize)), 0, CSng(PictureBox1.Width * (i / boardSize)), CSng(PictureBox1.Height))
            g.DrawLine(boardPen, 0, CSng(PictureBox1.Height * (i / boardSize)), CSng(PictureBox1.Width), CSng(PictureBox1.Height * (i / boardSize)))
        Next

        boardPen.Dispose()
        g.Dispose()
    End Sub

    Sub GameOver()
        Dim g As Graphics = PictureBox1.CreateGraphics
        Dim overImage As Image = Image.FromFile("C:\Users\callk\source\repos\Snake\SnakeGameoverScreen.png")

        g.DrawImage(overImage, Corner)
        g.DrawString("Score:" + CStr(snakeLength - 4), New Font("Arial", 16), New SolidBrush(Color.Black), New Point(100, 300))

        g.Dispose()
        overImage.Dispose()
    End Sub

    Private Sub WASD_Press(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        Dim keyPressed As String = e.KeyChar.ToString
        Select Case keyPressed
            Case "w"
                If currentDirection = Directions.DOWN Then
                    Return
                End If
                currentDirection = Directions.UP
            Case "a"
                If currentDirection = Directions.RIGHT Then
                    Return
                End If
                currentDirection = Directions.LEFT
            Case "s"
                If currentDirection = Directions.UP Then
                    Return
                End If
                currentDirection = Directions.DOWN
            Case "d"
                If currentDirection = Directions.LEFT Then
                    Return
                End If
                currentDirection = Directions.RIGHT
            Case vbCr
                If gameState = GameStates.PREGAME Then
                    gameState = GameStates.START
                ElseIf gameState = GameStates.STOPGAME Then
                    gameState = GameStates.PREGAME
                End If

        End Select
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Select Case gameState
            Case GameStates.PREGAME
                PreGame()
            Case GameStates.START
                StartGame()
            Case GameStates.RUN
                snakeDraw()
            Case GameStates.STOPGAME
                GameOver()
        End Select


    End Sub
End Class
