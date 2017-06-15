namespace TicTacToe

type public Space(Id : int) = class
    let mutable mark = '_'

    let validateMark mark =
        match mark with 
        | 'x' -> 'x'
        | 'o' -> 'o'
        | _ -> '_'

    member x.Id = Id
    member x.Mark = mark

    member x.SetMark(value) = mark <- validateMark value


end