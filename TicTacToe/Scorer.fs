namespace TicTacToe

module Scorer = 

    let getValue x =
        match x with
        | 'x' -> 1
        | 'o' -> -1
        | _ -> 0

    let getScoreForSpace (space:Space) =
        getValue space.Mark

    let getScoreForRow (spaces:List<Space>) =
        spaces |> List.sumBy(fun i -> getScoreForSpace i)