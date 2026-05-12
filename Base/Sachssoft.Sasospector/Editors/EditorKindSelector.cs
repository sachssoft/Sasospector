using System;
using System.Collections.Generic;
using System.Linq;

public sealed class EditorKindSelector
{
    private readonly Action<string?>? _changed;
    private readonly HashSet<string> _kinds;
    private readonly string _defaultKind;
    private string? _value;

    public EditorKindSelector(string defaultKind, Action<string?>? changed, params string[] kinds)
    {
        if (string.IsNullOrWhiteSpace(defaultKind))
            throw new ArgumentException(nameof(defaultKind));

        _defaultKind = defaultKind;
        _changed = changed;

        _kinds = new HashSet<string>(
            (kinds ?? Array.Empty<string>())
            .Where(x => !string.IsNullOrWhiteSpace(x))
        )
        {
            defaultKind
        };

        _value = null;
    }

    public IEnumerable<string> Kinds => _kinds.ToArray();

    public string? Value
    {
        get => _value;
        set
        {
            if (_value == value)
                return;

            _value = value;
            _changed?.Invoke(EffectiveValue);
        }
    }

    public bool IsDefault => _value is null;

    public bool IsResolved => _value is not null && _kinds.Contains(_value);

    public string EffectiveValue =>
        _value is null || !_kinds.Contains(_value)
            ? _defaultKind
            : _value;
}