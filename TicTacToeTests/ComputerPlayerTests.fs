namespace TicTacToeTests

    module ComputerPlayerTests =

        open NUnit.Framework
        open FsUnit

        open TicTacToe

        [<Test>]
        let ``ComputerPlayer has method to get next move depending on grid`` () =
            let fakeUtils = GameUtilities()
            let pcPlayer = ComputerPlayer(fakeUtils, MoveCalculator(GameUtilities()))
            let board = FakeSetup.testBoard
            let player = {Name = "John Did"; Mark = X}
            
            let nextMove : Space= pcPlayer.GetNextMove (board:Grid) (player:Player)

            nextMove.Marked |> should equal X
            

        [<Test>]
        let ``ComputerPlayer GetNextMove marks space based on player`` () =
            let fakeUtils = GameUtilities()
            let pcPlayer = ComputerPlayer(fakeUtils, MoveCalculator(GameUtilities()))
            let board = FakeSetup.testBoard
            let player = {Name = "John Did"; Mark = O}
            
            let nextMove : Space = pcPlayer.GetNextMove (board:Grid) (player:Player)
            
            nextMove.Marked |> should equal O
            

        [<Test>]
        let ``ComputerPlayer GetNextMove gets possible moves`` () =
            let fakeUtils = FakeGameUtilities((false:bool),(false:bool),false,false)
            let pcPlayer = ComputerPlayer(fakeUtils, FakeMoveCalculator())
            let board = FakeSetup.testBoard
            let player = {Name = "John Did"; Mark = O}
            
            let nextMove : Space= pcPlayer.GetNextMove (board:Grid) (player:Player)

            fakeUtils.PossibleMovesCallCount.Value > 0 |> should equal true

        [<Test>]
        let ``ComputerPlayer GetNextMove marks even if no way to win`` () =
            let fakeUtils = GameUtilities()
            let pcPlayer = ComputerPlayer(fakeUtils, MoveCalculator(GameUtilities()))
            let board = FakeSetup.losingBoard
            let player = {Name = "John Did"; Mark = O}
            
            let nextMove : Space = pcPlayer.GetNextMove (board:Grid) (player:Player)
            
            nextMove.Marked |> should equal O
            nextMove.Position |> should equal (Right,Center)

        [<Test>]
        let ``ComputerPlayer GetNextMove marks left center to block`` () =
            let fakeUtils = GameUtilities()
            let pcPlayer = ComputerPlayer(fakeUtils, MoveCalculator(GameUtilities()))
            let board = FakeSetup.needBlockBoard
            let player = {Name = "John Did"; Mark = O}
            
            let nextMove : Space = pcPlayer.GetNextMove (board:Grid) (player:Player)
                        
            nextMove.Marked |> should equal O
            nextMove.Position |> should equal (Left,Center)

        [<Test>]
        let ``ComputerPlayer GetNextMove marks right Bottom to block`` () =
            let fakeUtils = GameUtilities()
            let pcPlayer = ComputerPlayer(fakeUtils, MoveCalculator(GameUtilities()))
            let board = FakeSetup.needBlockBoard
            let player = {Name = "John Did"; Mark = O}

            let nextMove : Space = pcPlayer.GetNextMove (board:Grid) (player:Player)

            nextMove.Marked |> should equal O
            nextMove.Position |> should equal (Left, Center)

        [<Test>]
        let ``ComputerPlayer GetNextMove marks right Bottom to win`` () =
            let fakeUtils = GameUtilities()
            let pcPlayer = ComputerPlayer(fakeUtils, MoveCalculator(GameUtilities()))
            let board = FakeSetup.needBlockBoard2
            let player = {Name = "John Did"; Mark = X}

            let nextMove : Space = pcPlayer.GetNextMove (board:Grid) (player:Player)

            nextMove.Marked |> should equal X
            nextMove.Position |> should equal (Right,Bottom)

        [<Test>]
        let ``MoveCalculator GetScore returns a score of 10 for X if X has won`` () =
            let calc = MoveCalculator(FakeGameUtilities(false, true, true, false))
            let board = FakeSetup.XWinBoard
            let player = {Name = "Test"; Mark = X}
            
            let score = calc.GetScore board player 0

            score |> should equal 10

        [<Test>]
        let ``MoveCalculator GetScore returns a score of -10 for X if O has won`` () =
            let calc = MoveCalculator(FakeGameUtilities(false, true, false, true))
            let board = FakeSetup.OWinBoard
            let player = {Name = "Test"; Mark = X}
            
            let score = calc.GetScore board player 0

            score |> should equal -10

        [<Test>]
        let ``MoveCalculator GetScore returns a score of 0 for X if Tie`` () =
            let calc = MoveCalculator(FakeGameUtilities(true, false, false, false))
            let board = FakeSetup.fullBoard
            let player = {Name = "Test"; Mark = X}
            
            let score = calc.GetScore board player 0

            score |> should equal 0

        [<Test>]
        let ``MoveCalculator GetScore returns a score of 10 for O if O has won`` () =
            let calc = MoveCalculator(FakeGameUtilities(false, true, false, true))
            let board = FakeSetup.OWinBoard
            let player = {Name = "Test"; Mark = O}
            
            let score = calc.GetScore board player 0

            score |> should equal 10

        [<Test>]
        let ``MoveCalculator GetScore returns a score of -10 for O if X has won`` () =
            let calc = MoveCalculator(FakeGameUtilities(false, true, true, false))
            let board = FakeSetup.XWinBoard
            let player = {Name = "Test"; Mark = O}
            
            let score = calc.GetScore board player 0

            score |> should equal -10

        [<Test>]
        let ``MoveCalculator GetScore returns a score of 0 for O if Tie`` () =
            let calc = MoveCalculator(FakeGameUtilities(true, false, false, false))
            let board = FakeSetup.fullBoard
            let player = {Name = "Test"; Mark = O}
            
            let score = calc.GetScore board player 0

            score |> should equal 0

        [<Test>]
        let ``MoveCalculator GetScore adjusts score based on depth`` () =
            let calc = MoveCalculator(FakeGameUtilities(false, true, true, false))
            let board = FakeSetup.fullBoard
            let player = {Name = "Test"; Mark = O}
            
            let score = calc.GetScore board player 2

            score |> should equal -8

        [<Test>]
        let ``MoveCalculator GetOpponent returns a player with opposite mark`` () =
            let calc = MoveCalculator(FakeGameUtilities(true, false, false, false))
            let board = FakeSetup.fullBoard
            let player = {Name = "Test"; Mark = O}
            
            let opponent = calc.GetOpponent player

            opponent.Mark |> should equal X

        [<Test>]
        let ``MoveCalculator GetBestScore returns score if tie condition is met`` () =
            let calc = MoveCalculator(FakeGameUtilities(true, false, false, false))
            let board = FakeSetup.fullBoard
            let player = {Name = "Test"; Mark = O}
            
            let score = calc.GetBestScore board player 0 true

            score |> should equal 0

        [<Test>]
        let ``MoveCalculator GetBestScore returns score if win condition is met`` () =
            let calc = MoveCalculator(FakeGameUtilities(false, true, true, false))
            let board = FakeSetup.fullBoard
            let player = {Name = "Test"; Mark = X}
            
            let score = calc.GetBestScore board player 0 true

            score |> should equal 10