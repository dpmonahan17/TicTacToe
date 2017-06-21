namespace TicTacToeTests

    module ComputerPlayerTests =

        open NUnit.Framework
        open FsUnit

        open TicTacToe

        [<Test>]
        let ``ComputerPlayer has method to get next move depending on grid`` () =
            let fakeUtils = GameUtilities()
            let pcPlayer = ComputerPlayer(fakeUtils, Algorithm(GameUtilities()))
            let board = FakeSetup.testBoard
            let player = {Name = "John Did"; Mark = X}
            let nextMove : Space= pcPlayer.GetNextMove (board:Grid) (player:Player)
            match nextMove.Marked with
            | X -> printf "Mark is X\n"
            | O -> printf "Mark is O\n"
            | No -> printf "Mark is No\n"

            nextMove.Marked |> should equal X
            

        [<Test>]
        let ``ComputerPlayer GetNextMove marks space based on player`` () =
            let fakeUtils = GameUtilities()
            let pcPlayer = ComputerPlayer(fakeUtils, Algorithm(GameUtilities()))
            let board = FakeSetup.testBoard
            let player = {Name = "John Did"; Mark = O}
            let nextMove : Space = pcPlayer.GetNextMove (board:Grid) (player:Player)
            match nextMove.Marked with
            | X -> printf "Mark is X\n"
            | O -> printf "Mark is O\n"
            | No -> printf "Mark is No\n"
            
            nextMove.Marked |> should equal O
            

        [<Test>]
        let ``ComputerPlayer GetNextMove gets possible moves`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
            let pcPlayer = ComputerPlayer(fakeUtils, fakeAlgorithm(fakeUtils))
            let board = FakeSetup.testBoard
            let player = {Name = "John Did"; Mark = O}
            let nextMove : Space= pcPlayer.GetNextMove (board:Grid) (player:Player)

            fakeUtils.PossibleMovesCallCount.Value > 0 |> should equal true

        [<Test>]
        let ``ComputerPlayer GetNextMove marks even if no way to win`` () =
            let fakeUtils = GameUtilities()
            let pcPlayer = ComputerPlayer(fakeUtils, Algorithm(GameUtilities()))
            let board = FakeSetup.losingBoard
            let player = {Name = "John Did"; Mark = O}
            let nextMove : Space = pcPlayer.GetNextMove (board:Grid) (player:Player)
            match nextMove.Marked with
            | X -> printf "Mark is X\n"
            | O -> printf "Mark is O\n"
            | No -> printf "Mark is No\n"
            
            nextMove.Marked |> should equal O
            nextMove.Position |> should equal (Right,Center)

        [<Test>]
        let ``ComputerPlayer GetNextMove marks left center to block`` () =
            let fakeUtils = GameUtilities()
            let pcPlayer = ComputerPlayer(fakeUtils, Algorithm(GameUtilities()))
            let board = FakeSetup.needBlockBoard
            let player = {Name = "John Did"; Mark = O}
            let nextMove : Space = pcPlayer.GetNextMove (board:Grid) (player:Player)
            match nextMove.Marked with
            | X -> printf "Mark is X\n"
            | O -> printf "Mark is O\n"
            | No -> printf "Mark is No\n"
                        
            nextMove.Marked |> should equal O
            nextMove.Position |> should equal (Left,Center)