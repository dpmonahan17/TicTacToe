namespace TicTacToeTests

    module GameTests =

        open NUnit.Framework
        open FsUnit

        open TicTacToe

        let buiLdXTurn =
            let spaces = [
                    {Position = (Left, Top); Marked = No};
                    {Position = (Left, Center); Marked = No}]
            let players = {PlayerX = {Name = "John Doe"; Mark = X};
                           PlayerO = {Name = "John Did"; Mark = O}}
            XTurn {grid = {grid = spaces}; players = players}
        
        let buiLdOTurn =
            let spaces = [
                    {Position = (Left, Top); Marked = No};
                    {Position = (Left, Center); Marked = No}]
            let players = {PlayerX = {Name = "John Doe"; Mark = X};
                           PlayerO = {Name = "John Did"; Mark = O}}
            OTurn {grid = {grid = spaces}; players = players}
        

        [<Test>]
        let ``Game should print game end results if game is completed`` () =
            let fake = fakeGameControls()
            let utilities = GameUtilities()
            let game = Game(fake, utilities)                    
            let board = 
                Completed {player = {Name = "John Doe"; Mark = X}; result = Win }
            
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
                player.Mark |> should equal X
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
                player.Mark |> should equal X
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
            let board = Completed {player = {Name = "John Doe"; Mark = X}; result = Win }
            
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
            let board = Completed {player = {Name = "John Doe"; Mark = X}; result = Win }
            
            let result = board |> game.PerformTurn
            match result with
            | Completed {player=player; result=resultData} -> result |> should equal board
            | _ -> result |> should not' (equal board)

            fakeControls.CallGetTurnCount.Value |> should equal 0