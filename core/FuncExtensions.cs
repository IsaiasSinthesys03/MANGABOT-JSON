/// <summary>
/// Provee metodos de extension para combinar funciones booleanas (predicados).
/// Util para construir filtros complejos de forma legible.
/// super duper comands
/// </summary>
public static class FuncExtensions
{
    /// <summary>
    /// Combina dos funciones con una operacion AND logica.
    /// </summary>
    /// <typeparam name="T">El tipo de entrada de las funciones.</typeparam>
    /// <param name="left">La primera funcion (predicado).</param>
    /// <param name="right">La segunda funcion (predicado).</param>
    /// <returns>Una nueva funcion que devuelve true si ambas funciones son true.</returns>
    public static Func<T, bool> And<T>(this Func<T, bool> left, Func<T, bool> right)
    {
        return x => left(x) && right(x);
    }

    /// <summary>
    /// Combina dos funciones con una operacion OR logica.
    /// </summary>
    /// <typeparam name="T">El tipo de entrada de las funciones.</typeparam>
    /// <param name="left">La primera funcion (predicado).</param>
    /// <param name="right">La segunda funcion (predicado).</param>
    /// <returns>Una nueva funcion que devuelve true si al menos una funcion es true.</returns>
    public static Func<T, bool> Or<T>(this Func<T, bool> left, Func<T, bool> right)
    {
        return x => left(x) || right(x);
    }

    /// <summary>
    /// Invierte el resultado de una funcion (operacion NOT logica).
    /// </summary>
    /// <typeparam name="T">El tipo de entrada de la funcion.</typeparam>
    /// <param name="func">La funcion (predicado) a invertir.</param>
    /// <returns>Una nueva funcion que devuelve el resultado opuesto de la funcion original.</returns>
    public static Func<T, bool> Not<T>(this Func<T, bool> func)
    {
        return x => !func(x);
    }
}