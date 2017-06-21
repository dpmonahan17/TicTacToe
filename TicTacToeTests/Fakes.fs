namespace TicTacToeTests

open NUnit.Framework
open FsUnit

open TicTacToe

type fakeAlgorithm(gameUtil : IGameUtilities) = class

    member x.gameUtil = gameUtil

    interface IAlgorithm with
        member x.GetBestScore (grid:Grid) (player:Player) (depth:int) (maximizingPlayer:bool) =
            10

end

type fakeGameUtilities(tieResult:bool, winResult:bool) = class
    let mutable tieCallCount = ref 0
    let mutable winCallCount = ref 0
    let mutable possibleMovesCallCount = ref 0
    let mutable tieResult = tieResult
    let mutable winResult = winResult
    
    member x.TieCallCount:int ref = tieCallCount
    member x.WinCallCount:int ref = winCallCount
    member x.PossibleMovesCallCount = possibleMovesCallCount
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
            incr possibleMovesCallCount
            [{Position = (Left, Top); Marked = No};
            {Position = (Left, Center); Marked = X}]
        
        member x.MarkMove (move:Space) (player:Player) =
            move

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

        member x.ChooseMark =
            X

        member x.GetPlayerData =
            {Name = "Jim Bob"; Mark = X}

end

module FakeSetup =
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

    let losingBoard : Grid =
        {grid = [
            {Position = (Left, Top); Marked = X};
            {Position = (Left, Center); Marked = X};
            {Position = (Left, Bottom); Marked = O};
            {Position = (Middle, Top); Marked = O};
            {Position = (Middle, Center); Marked = O};
            {Position = (Middle, Bottom); Marked = X};
            {Position = (Right, Top); Marked = X};
            {Position = (Right, Center); Marked = No};
            {Position = (Right, Bottom); Marked = No};
        ]}

    let needBlockBoard : Grid =
        {grid = [
            {Position = (Left, Top); Marked = X};
            {Position = (Left, Center); Marked = No};
            {Position = (Left, Bottom); Marked = X};
            {Position = (Middle, Top); Marked = O};
            {Position = (Middle, Center); Marked = No};
            {Position = (Middle, Bottom); Marked = No};
            {Position = (Right, Top); Marked = No};
            {Position = (Right, Center); Marked = No};
            {Position = (Right, Bottom); Marked = No};
        ]}