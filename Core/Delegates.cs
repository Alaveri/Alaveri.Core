namespace Alaveri.Core;

/// <summary>
/// Represents a void function that takes an argument and an index.
/// </summary>
/// <typeparam name="TArg">The type of the argument.</typeparam>
/// <param name="arg">The argument.</param>
/// <param name="index">The index.</param>
public delegate void Proc<TArg>(TArg arg, int index);
