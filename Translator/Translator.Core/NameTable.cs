using System;
using System.Collections.Generic;

public enum tCat
{
    Const,
    Var,
    Type
}

public enum tType
{
    None,
    Int,
    Bool
}

public struct Identifier
{
    public string Name;
    public tType Type;
    public tCat Category;

    public Identifier(string name, tType type, tCat category)
    {
        Name = name;
        Type = type;
        Category = category;
    }
}


public class NameTable
{
    private List<Identifier> identifiers;

    public NameTable()
    {
        identifiers = new List<Identifier>();
    }

    public Identifier AddIdentifier(string name, tCat category)
    {
        if (FindByName(name).Equals(default(Identifier)))
        {
            Identifier identifier = new Identifier
            {
                Name = name,
                Category = category,
                Type = tType.None // Или установите тип по умолчанию
            };

            identifiers.Add(identifier);
            return identifier;
        }
        else
        {
            throw new Exception($"Ошибка: Идентификатор с именем '{name}' уже существует.");
        }
    }

    public Identifier FindByName(string name)
    {
        foreach (var id in identifiers)
        {
            if (id.Name == name)
            {
                return id;
            }
        }
        return default; // Возврат по умолчанию, если идентификатор не найден
    }

    public List<Identifier> GetIdentifiers() => identifiers;

    public void Initialize()
    {
        identifiers = new List<Identifier>();
    }
}
