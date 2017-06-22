namespace TicTacToeTests

    module ComputerPlayerTests =

        open NUnit.Framework
        open FsUnit

        open TicTacToe

        let printPosition (position:GridPosition) =
            match position with
                | (Left, Top) -> printf "\nPosition is Left, Top"
                | (Left, Center) -> printf "\nPosition is Left, Center"
                | (Left, Bottom) -> printf "\nPosition is Left, Bottom"
                | (Middle, Top) -> printf "\nPosition is Middle, Top"
                | (Middle, Center) -> printf "\nPosition is Middle, Center"
                | (Middle, Bottom) -> printf "\nPosition is Middle, Bottom"
                | (Right, Top) -> printf "\nPosition is Right, Top"
                | (Right, Center) -> printf "\nPosition is Right, Center"
                | (Right, Bottom) -> printf "\nPosition is Right, Bottom"

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
            let fakeUtils = FakeGameUtilities((false:bool),(false:bool))
            let pcPlayer = ComputerPlayer(fakeUtils, FakeAlgorithm(fakeUtils))
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

        [<Test>]
        let ``ComputerPlayer GetNextMove marks right Bottom to block`` () =
            let fakeUtils = GameUtilities()
            let pcPlayer = ComputerPlayer(fakeUtils, Algorithm(GameUtilities()))
            let board = FakeSetup.needBlockBoard
            let player = {Name = "John Did"; Mark = O}

            let nextMove : Space = pcPlayer.GetNextMove (board:Grid) (player:Player)

            match nextMove.Marked with
            | X -> printf "Mark is X\n"
            | O -> printf "Mark is O\n"
            | No -> printf "Mark is No\n"
            printPosition nextMove.Position                                

            nextMove.Marked |> should equal O
            nextMove.Position |> should equal (Left, Center)

        [<Test>]
        let ``ComputerPlayer GetNextMove marks right Bottom to win`` () =
            let fakeUtils = GameUtilities()
            let pcPlayer = ComputerPlayer(fakeUtils, Algorithm(GameUtilities()))
            let board = FakeSetup.needBlockBoard2
            let player = {Name = "John Did"; Mark = X}

            let nextMove : Space = pcPlayer.GetNextMove (board:Grid) (player:Player)

            match nextMove.Marked with
            | X -> printf "Mark is X\n"
            | O -> printf "Mark is O\n"
            | No -> printf "Mark is No\n"
            printPosition nextMove.Position                                

            nextMove.Marked |> should equal X
            nextMove.Position |> should equal (Right,Bottom)