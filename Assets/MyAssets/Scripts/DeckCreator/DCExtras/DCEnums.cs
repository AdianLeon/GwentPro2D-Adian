using System.Collections;
using System.Collections.Generic;
//Script que declara los enums a utilizar en el DeckCreator
public enum TokenTypes{//Tipos de tokens
     number,// ... -3 -2 -1 0 1 2 3 ...
     literal,// "Un string"

     //Identificadores
          identifier,
          assignment,// Name Params Action Type Effect Selector Source Single Predicate PostAction ScriptEffect
          cardAssignment,// Faction Power Range OnActivation
          cycle,// for in while
          varType,// Number String Bool
          blockDeclaration,//card effect
     //

     //Simples
          punctuator,// : ; , . " '
          limitator,// ( ) [ ] { }
          //Operators
               booleanOp,// == != < > <= >= && ||
               arithemticOp,// + - * / ^ += -= *= /= ^=
               concatOp,// @ @@
               asignationOp,//=
               unaryOp,// ++ --
               lambdaOp,// =>
     //

     //Otros
          end,// $
          unexpected// ?
     //
};
