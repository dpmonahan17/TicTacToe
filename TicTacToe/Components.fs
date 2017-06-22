namespace TicTacToe

type Mark = X | O | No

type GameResult = Win | Tie

type Player = {Name: string
               Mark : Mark}

type Players = {PlayerX : Player
                PlayerO : Player}

type GridRow = Left | Middle | Right
type GridColumn = Top | Center | Bottom
type GridPosition = GridRow * GridColumn

type Space = {
    Position : GridPosition
    Marked : Mark
}

type Grid = {
    grid: Space List
}

type BoardData = {
    grid : Grid
    players : Players
}

type GameResultsData = {
    grid : Grid
    player : Player
    result : GameResult
}

type IBoardState = 
    | XTurn of BoardData
    | OTurn of BoardData
    | Completed of GameResultsData
