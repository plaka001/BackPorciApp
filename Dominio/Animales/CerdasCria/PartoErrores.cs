using Dominio.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Animales;


public class PartoErrores
{
    public static Error PartoExistente = new(
     "Parto.Existente",
     "Ya el parto"
    );

    public static Error NoEncontrada = new(
        "Parto.NoEncontrado",
        "El parto no se ha encontrado"
        );
}