namespace TicTacToeTests

open NUnit.Framework
open FsUnit

open TicTacToe

type FakeMoveCalculator() = class

    interface IMoveCalculator with
        member x.GetBestScore (grid:Grid) (player:Player) (depth:int) (maximizingPlayer:bool) =
            10

end

type FakeComputerPlayer(position : GridPosition, mark : Mark) = class

    interface IComputerPlayer with

        member x.GetNextMove (grid:Grid) (player:Player) =
            {Position = position; Marked = mark}

end

type FakeGameUtilities(tieResult:bool, winResult:bool, xWin:bool, oWin:bool) = class
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
            xWin

        member x.CheckForOWin grid =
            oWin

        member x.GetPossibleMoves (grid:Grid) =
            incr possibleMovesCallCount
            [{Position = (Left, Top); Marked = No};
            {Position = (Left, Center); Marked = X}]
        
        member x.MarkMove (move:Space) (player:Player) =
            move

end

type FakeGameControls() = class
    
    let mutable callCount = ref 0
    let mutable callGetTurnCount = ref 0
                
    member x.CallCount:int ref = callCount
    member x.CallGetTurnCount = callGetTurnCount

    interface IGameControls with
                    
        member x.PrintGameResults (grid:Grid) (results:GameResult) (player:Player) =
            incr callCount
            
        member x.GetTurn grid player=
            incr callGetTurnCount
            {grid = [
                        {Position = (Left, Top); Marked = No};
                        {Position = (Left, Center); Marked = No};
                        {Position = (Left, Bottom); Marked = X}
                    ]
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

    let needBlockBoard2 : Grid =
        {grid = [
                    {Position = (Left, Top); Marked = X};
                    {Position = (Left, Center); Marked = O};
                    {Position = (Left, Bottom); Marked = No};
                    {Position = (Middle, Top); Marked = O};
                    {Position = (Middle, Center); Marked = O};
                    {Position = (Middle, Bottom); Marked = No};
                    {Position = (Right, Top); Marked = X};
                    {Position = (Right, Center); Marked = X};
                    {Position = (Right, Bottom); Marked = No};
                ]}

    let XWinBoard : Grid =
        {grid = [
                    {Position = (Left, Top); Marked = X};
                    {Position = (Left, Center); Marked = O};
                    {Position = (Left, Bottom); Marked = No};
                    {Position = (Middle, Top); Marked = X};
                    {Position = (Middle, Center); Marked = O};
                    {Position = (Middle, Bottom); Marked = No};
                    {Position = (Right, Top); Marked = X};
                    {Position = (Right, Center); Marked = X};
                    {Position = (Right, Bottom); Marked = No};
                ]}
    
    let OWinBoard : Grid =
        {grid = [
                    {Position = (Left, Top); Marked = O};
                    {Position = (Left, Center); Marked = O};
                    {Position = (Left, Bottom); Marked = O};
                    {Position = (Middle, Top); Marked = X};
                    {Position = (Middle, Center); Marked = O};
                    {Position = (Middle, Bottom); Marked = No};
                    {Position = (Right, Top); Marked = X};
                    {Position = (Right, Center); Marked = X};
                    {Position = (Right, Bottom); Marked = No};
                ]}

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