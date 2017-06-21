namespace TicTacToeTests

    module ComputerPlayerTests =

        open NUnit.Framework
        open FsUnit

        open TicTacToe

        // [<Test>]
        // let ``GetMoveScore returns -10 if move wins`` () =
        //     let fakeUtils = fakeGameUtilities((false:bool), (false:bool))
        //     let pcPlayer = ComputerPlayer(fakeUtils)
        //     let grid = FakeSetup.testBoard
        //     let move = {Position = (Left, Top); Marked = X}
            
        //     let result = pcPlayer.GetMoveScore grid move

        //     result |> should equal -10

        // [<Test>]
        // let ``CheckTie returns NextMove if board is not full`` () =
        //     let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
        //     let pcPlayer = ComputerPlayer(fakeUtils)
        //     let moveData = NextMove {grid = FakeSetup.testBoard;
        //                     move = {Position = (Left, Top); Marked = X};
        //                     score = 0;
        //                     depth = 0}
        //     let nextState = pcPlayer.CheckTie moveData
        //     let result = 
        //         match nextState with
        //         | NextMove {grid=grid; move=move; score=score; depth=depth}
        //             -> true
        //         | _ 
        //             -> false
        //     result |> should equal true

        // [<Test>]
        // let ``NextMove returns EndMove if board is full`` () =
        //     let fakeUtils = fakeGameUtilities((true:bool),(false:bool))
        //     let pcPlayer = ComputerPlayer(fakeUtils)
        //     let moveData = NextMove {grid = FakeSetup.fullBoard;
        //                     move = {Position = (Left, Top); Marked = X};
        //                     score = 0;
        //                     depth = 0}
        //     let nextState = pcPlayer.CheckTie moveData
        //     let result = 
        //         match nextState with
        //         | EndMove {grid=grid; move=move; score=score; depth=depth}
        //             -> true
        //         | _ 
        //             -> false
        //     result |> should equal true

        // [<Test>]
        // let ``Scorer can get the best move`` () =
        //     let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
        //     let pcPlayer = ComputerPlayer(fakeUtils)
        //     let grid = testBoard
        //     (grid.grid |> List.find(fun i -> i.Position = (Left, Top))).Marked |> should equal No
        //     let result = pcPlayer.GetBestMove grid
        //     (result.grid |> List.find(fun i -> i.Position = (Left, Top))).Marked |> should equal X


        // [<Test>]
        // let ``NextMove returns TieMove if board is full`` () =
        //     let fakeUtils = fakeGameUtilities((true:bool),(false:bool))
        //     let pcPlayer = ComputerPlayer(fakeUtils)
        //     let moveData = NextMove {grid = FakeSetup.fullBoard;
        //                     move = {Position = (Left, Top); Marked = X};
        //                     score = 0;
        //                     depth = 0}
        //     let nextState = pcPlayer.NextMove moveData
        //     let result = 
        //         match nextState with
        //         | TieMove {grid=grid; move=move; score=score; depth=depth}
        //             -> true
        //         | _ 
        //             -> false
        //     result |> should equal true