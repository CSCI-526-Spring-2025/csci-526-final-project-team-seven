using System;

[Serializable]
public class FirestoreList
{
    public Entry[] entries;
}

[Serializable]
public class Entry
{
    public Document document;
}

[Serializable]
public class Document
{
    public Fields fields;
}

[Serializable]
public class Fields
{
    public StringValue name;
    public IntValue wave;
    public IntValue killed;
    public IntValue revive;
}

[Serializable]
public class StringValue
{
    public string stringValue;
}

[Serializable]
public class IntValue
{
    // Firestore 里 integerValue 存的是字符串，所以这里也先用 string
    public string integerValue;
}
