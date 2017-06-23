namespace TicTacToeTests

    module GameControlsTests =

        open NUnit.Framework
        open FsUnit

        open TicTacToe

        [<Test>]
        let ``MarkToString returns the string value of a mark`` () =
            let gameControls = GameControls(FakeGameUtilities(false,false,false,false), FakeComputerPlayer((Left,Center), X))

            let result = gameControls.MarkToString X

            result |> should equal "x"

        [<Test>]
        let ``GetPositionPerIndex returns a position based on a number`` () =
            let gameControls = GameControls(FakeGameUtilities(false,false,false,false), FakeComputerPlayer((Left,Center), X))

            let result = gameControls.GetPositionPerIndex 1
            let result2 = gameControls.GetPositionPerIndex 4

            result |> should equal (Left,Top)
            result2 |> should equal (Left,Center)

        [<Test>]
        let ``GetMark returns the mark from a grid based on index`` () =
            let gameControls = GameControls(FakeGameUtilities(false,false,false,false), FakeComputerPlayer((Left,Center), X))
            let grid = FakeSetup.testBoard

            let result = gameControls.GetMark grid 1
            
            result |> should equal " "

        [<Test>]
        let ``IsInvalidMove returns true if move is already marked`` () =
            let gameControls = GameControls(FakeGameUtilities(false,false,false,false), FakeComputerPlayer((Left,Center), X))
            let grid = FakeSetup.testBoard

            let result = gameControls.IsInValidMove grid (Left,Bottom)
            let result2 = gameControls.IsInValidMove grid (Left,Center)

            result |> should equal true
            result2 |> should equal false