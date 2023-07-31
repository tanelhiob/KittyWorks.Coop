module Utilities

type Change<'left, 'right> =
| Addition of 'right
| Update of 'left * 'right
| Deletion of 'left

let compare<'left, 'right> (left: 'left seq) (right: 'right seq) (compare: 'left -> 'right -> int): Change<'left, 'right> seq =
    Seq.empty<Change<'left, 'right>>