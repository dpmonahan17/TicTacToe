namespace TicTacToeTests

    module GameModuleTests =

        open NUnit.Framework
        open FsUnit

        open TicTacToe
    

        type fakeGameUtilities(tieResult:bool, winResult:bool) = class
            let mutable tieCallCount = ref 0
            let mutable winCallCount = ref 0
            let mutable tieResult = tieResult
            let mutable winResult = winResult
            
            member x.TieCallCount:int ref = tieCallCount
            member x.WinCallCount:int ref = winCallCount
            member x.TieResult:bool = tieResult            
            member x.WinResult:bool = winResult

            interface IGameUtilities with
                member x.UpdateGrid (grid:Grid) (selection:Space) =
                    let position = selection.Position
                    let newGrid = 
                        grid.grid |> List.map(fun i ->
                            if i.Position = position 
                            then selection
                            else i
                            )
                    {grid = newGrid} :Grid

                member x.CheckForTie board =
                    incr tieCallCount
                    tieResult
                member x.CheckForWin board =
                    incr winCallCount
                    winResult
                member x.BuildBlankBoard = 
                    let grid:Grid =
                        {grid = []}
                    grid

                member x.GetSpace grid position =
                    {Position = (Left, Top); Marked = No}
                
                member x.GetRow grid index = 
                    [{Position = (Left, Top); Marked = No};
                    {Position = (Left, Center); Marked = X};
                    {Position = (Left, Bottom); Marked = O}]
                
                member x.CheckForXWin grid =
                    false

                member x.CheckForOWin grid =
                    false

                member x.GetPossibleMoves (grid:Grid) =
                    let grid2:Grid =
                        {grid = 
                            [{Position = (Left, Top); Marked = No};
                            {Position = (Left, Center); Marked = X}]
                        }
                    grid2
        end

        type fakeGameControls() = class
            
            let mutable callCount = ref 0
            let mutable callGetTurnCount = ref 0
                        
            member x.CallCount:int ref = callCount
            member x.CallGetTurnCount = callGetTurnCount

            interface IGameControls with
                            
                member x.PrintGameResults (results:GameResult) (player:Player) =
                    incr callCount
                    
                member x.GetTurn grid player=
                    incr callGetTurnCount
                    {grid = [
                        {Position = (Left, Top); Marked = No};
                        {Position = (Left, Center); Marked = No};
                        {Position = (Left, Bottom); Marked = X}]
                    }

        end


        let buiLdXTurn =
            let spaces = [
                    {Position = (Left, Top); Marked = No};
                    {Position = (Left, Center); Marked = No}]
            let players = {PlayerX = {name = "John Doe"; mark = X};
                           PlayerO = {name = "John Did"; mark = O}}
            XTurn {grid = {grid = spaces}; players = players}
        
        let buiLdOTurn =
            let spaces = [
                    {Position = (Left, Top); Marked = No};
                    {Position = (Left, Center); Marked = No}]
            let players = {PlayerX = {name = "John Doe"; mark = X};
                           PlayerO = {name = "John Did"; mark = O}}
            OTurn {grid = {grid = spaces}; players = players}
        

        [<Test>]
        let ``Game should print game end results if game is completed`` () =
            let fake = fakeGameControls()
            let utilities = GameUtilities()
            let game = Game(fake, utilities)                    
            let board = 
                Completed {player = {name = "John Doe"; mark = X}; result = Win }
            
            let result = board |> game.printResults 
            match result with
            | Completed {player=player; result=resultData} -> result |> should equal board
            | _ -> result |> should equal board

            fake.CallCount.Value |> should equal 1
        
        [<Test>]
        let ``Game should not print end game results if its X's turn`` () =
            let fake = fakeGameControls()
            let utilities = GameUtilities()
            let game = Game(fake, utilities)
            let board = buiLdXTurn
            
            let result = board |> game.printResults 
            match result with
            | Completed {player=player; result=resultData} -> resultData |> should equal board
            | _ -> result |> should equal board

            fake.CallCount.Value |> should equal 0

        [<Test>]
        let ``Game should not print end game results if its O's turn`` () =
            let fake = fakeGameControls()
            let utilities = GameUtilities()
            let game = Game(fake, utilities)
            let board = buiLdOTurn
            
            let result = board |> game.printResults 
            match result with
            | Completed {player=player; result=resultData} -> resultData |> should equal board
            | _ -> result |> should equal board

            fake.CallCount.Value |> should equal 0

        [<Test>]
        let ``Game CheckForTie step should return Completed if X Turn and game is tied`` () =
            let fakeUtils = fakeGameUtilities((true:bool),(false:bool))
            let game = Game(fakeGameControls(), fakeUtils)
            let board = buiLdXTurn
            
            let result = board |> game.CheckForTie
            match result with
            | Completed {player=player; result=resultData} -> resultData |> should equal Tie
            | _ -> result |> should equal board

            fakeUtils.TieCallCount.Value |> should equal 1

        [<Test>]
        let ``Game CheckForTie step should return original board if game is not tied`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
            let game = Game(fakeGameControls(), fakeUtils)
            let board = buiLdOTurn
            
            let result = board |> game.CheckForTie
            match result with
            | Completed {player=player; result=resultData} -> resultData |> should equal Tie
            | _ -> result |> should equal board

            fakeUtils.TieCallCount.Value |> should equal 1

        [<Test>]
        let ``Game CheckForWin step should return original board if game is not Won`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
            let game = Game(fakeGameControls(), fakeUtils)
            let board = buiLdOTurn
            
            let result = board |> game.CheckForWin
            match result with
            | Completed {player=player; result=resultData} -> 
                resultData |> should equal Tie
                player.mark |> should equal X
            | _ -> result |> should equal board

            fakeUtils.WinCallCount.Value |> should equal 1

        [<Test>]
        let ``Game CheckForWin step should return Completed With win and player if game is Won`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(true:bool))
            let game = Game(fakeGameControls(), fakeUtils)
            let board = buiLdXTurn
            
            let result = board |> game.CheckForWin
            match result with
            | Completed {player=player; result=resultData} -> 
                resultData |> should equal Win
                player.mark |> should equal X
            | _ -> board |> should equal board

            fakeUtils.WinCallCount.Value |> should equal 1

        [<Test>]
        let ``Game PerformTurn step should return Board with updated Grid if player turn`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
            let fakeControls = fakeGameControls()
            let game = Game(fakeControls, fakeUtils)
            let board = buiLdXTurn
            
            let result = board |> game.PerformTurn
            match result with
            | Completed {player=player; result=resultData} -> result |> should equal board
            | _ -> result |> should not' (equal board)

            fakeControls.CallGetTurnCount.Value |> should equal 1

        [<Test>]
        let ``Game PerformTurn step should return Board if already Completed`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
            let fakeControls = fakeGameControls()
            let game = Game(fakeControls, fakeUtils)
            let board = Completed {player = {name = "John Doe"; mark = X}; result = Win }
            
            let result = board |> game.PerformTurn
            match result with
            | Completed {player=player; result=resultData} -> result |> should equal board
            | _ -> result |> should not' (equal board)

            fakeControls.CallGetTurnCount.Value |> should equal 0

        [<Test>]
        let ``Game NextTurn step call the other steps`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
            let fakeControls = fakeGameControls()
            let game = Game(fakeControls, fakeUtils)
            let board = Completed {player = {name = "John Doe"; mark = X}; result = Win }
            
            let result = board |> game.PerformTurn
            match result with
            | Completed {player=player; result=resultData} -> result |> should equal board
            | _ -> result |> should not' (equal board)

            fakeControls.CallGetTurnCount.Value |> should equal 0

        let testBoard :Grid =
            {grid = [
                {Position = (Left, Top); Marked = No};
                {Position = (Left, Center); Marked = No};
                {Position = (Left, Bottom); Marked = X};
                {Position = (Middle, Top); Marked = O};
                {Position = (Middle, Center); Marked = X};
                {Position = (Middle, Bottom); Marked = O};
                {Position = (Right, Top); Marked = O};
                {Position = (Right, Center); Marked = O};
                {Position = (Right, Bottom); Marked = X};
            ]}
        
        let fullBoard : Grid =
            {grid = [
                {Position = (Left, Top); Marked = O};
                {Position = (Left, Center); Marked = X};
                {Position = (Left, Bottom); Marked = X};
                {Position = (Middle, Top); Marked = X};
                {Position = (Middle, Center); Marked = X};
                {Position = (Middle, Bottom); Marked = O};
                {Position = (Right, Top); Marked = O};
                {Position = (Right, Center); Marked = O};
                {Position = (Right, Bottom); Marked = X};
            ]}


        [<Test>]
        let ``GetScoreForSpace can get the score of one space`` () =
            let gameUtils = GameUtilities()
            let space1 = 
                {Position = (Left, Top); Marked = No}
            let space2 = 
                {Position = (Left, Center); Marked = X}
            let space3 = 
                {Position = (Left, Bottom); Marked = O}
                
            let resultNo = gameUtils.GetScoreForSpace space1
            let resultX = gameUtils.GetScoreForSpace space2
            let resultO = gameUtils.GetScoreForSpace space3

            resultNo |> should equal 0
            resultX |> should equal 1
            resultO |> should equal -1


        [<Test>]
        let ``GetScoreForRow can get the score of one row`` () =
            let gameUtils = GameUtilities()
            let grid = testBoard.grid |> List.filter(fun i -> i.Position = (Left, Top) || i.Position = (Left, Center) || i.Position = (Left, Bottom))
            
            let result = gameUtils.GetScoreForRow grid

            result |> should equal 1

        [<Test>]
        let ``GetMoveScore returns -10 if move wins`` () =
            let fakeUtils = fakeGameUtilities((false:bool), (false:bool))
            let scorer = GameScorer(fakeUtils)
            let grid = testBoard
            let move = {Position = (Left, Top); Marked = X}
            
            let result = scorer.GetMoveScore grid move

            result |> should equal -10


        [<Test>]
        let ``Scorer can get the best move`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
            let scorer = GameScorer(fakeUtils)
            let grid = testBoard
            (grid.grid |> List.find(fun i -> i.Position = (Left, Top))).Marked |> should equal No
            let result = scorer.GetBestMove grid
            (result.grid |> List.find(fun i -> i.Position = (Left, Top))).Marked |> should equal X


        [<Test>]
        let ``GetPossibleMoves returns grid of valid spaces`` () =
            let utilities = GameUtilities()
            let newGrid : Grid = utilities.GetPossibleMoves testBoard
            
            newGrid.grid.Length |> should equal 2
            (newGrid.grid |> List.find(fun i -> i.Position = (Left, Top))).Marked |> should equal No
            (newGrid.grid |> List.find(fun i -> i.Position = (Left, Center))).Marked |> should equal No

        [<Test>]
        let ``GetSpace returns specific space`` () =
            let utilities = GameUtilities()
            let space : Space = utilities.GetSpace testBoard (Middle, Center)
            
            space.Marked |> should equal X
            space.Position |> should equal (Middle, Center)

        [<Test>]
        let ``GetRow returns specific row`` () =
            let utilities = GameUtilities()
            let grid = testBoard
            
            let result = utilities.GetRow grid 1
            
            (result |> List.find(fun i -> i.Position = (Left, Top))).Marked |> should equal No
            (result |> List.find(fun i -> i.Position = (Left, Center))).Marked |> should equal No
            (result |> List.find(fun i -> i.Position = (Left, Bottom))).Marked |> should equal X
            
            let result2 = utilities.GetRow grid 4

            (result2 |> List.find(fun i -> i.Position = (Right, Top))).Marked |> should equal O
            (result2 |> List.find(fun i -> i.Position = (Middle, Top))).Marked |> should equal O
            (result2 |> List.find(fun i -> i.Position = (Left, Top))).Marked |> should equal No
            
        [<Test>]
        let ``UpdateGrid returns Grid with modified space`` () =
            let utilities = GameUtilities()
            let grid = testBoard
            (grid.grid |> List.find(fun i -> i.Position = (Left,Bottom))).Marked |> should equal X
            (grid.grid |> List.find(fun i -> i.Position = (Left,Top))).Marked |> should equal No

            let newGrid = utilities.UpdateGrid testBoard {Position = (Left, Top); Marked = X}

            (newGrid.grid |> List.find(fun i -> i.Position = (Left,Bottom))).Marked |> should equal X
            (newGrid.grid |> List.find(fun i -> i.Position = (Left,Top))).Marked |> should equal X
            newGrid.grid.Length |> should equal 9

        [<Test>]
        let ``CheckTie returns NextMove if board is not full`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
            let scorer = GameScorer(fakeUtils)
            let moveData = NextMove {grid = testBoard;
                            move = {Position = (Left, Top); Marked = X};
                            score = 0;
                            depth = 0}
            let nextState = scorer.CheckTie moveData
            let result = 
                match nextState with
                | NextMove {grid=grid; move=move; score=score; depth=depth}
                    -> true
                | _ 
                    -> false
            result |> should equal true

        [<Test>]
        let ``NextMove returns TieMove if board is full`` () =
            let fakeUtils = fakeGameUtilities((true:bool),(false:bool))
            let scorer = GameScorer(fakeUtils)
            let moveData = NextMove {grid = fullBoard;
                            move = {Position = (Left, Top); Marked = X};
                            score = 0;
                            depth = 0}
            let nextState = scorer.CheckTie moveData
            let result = 
                match nextState with
                | TieMove {grid=grid; move=move; score=score; depth=depth}
                    -> true
                | _ 
                    -> false
            result |> should equal true

        // [<Test>]
        // let ``NextMove returns TieMove if board is full`` () =
        //     let fakeUtils = fakeGameUtilities((true:bool),(false:bool))
        //     let scorer = GameScorer(fakeUtils)
        //     let moveData = NextMove {grid = fullBoard;
        //                     move = {Position = (Left, Top); Marked = X};
        //                     score = 0;
        //                     depth = 0}
        //     let nextState = scorer.NextMove moveData
        //     let result = 
        //         match nextState with
        //         | TieMove {grid=grid; move=move; score=score; depth=depth}
        //             -> true
        //         | _ 
        //             -> false
        //     result |> should equal true