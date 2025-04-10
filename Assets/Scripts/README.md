
# Code Style Guide

- Use the standard C# camelCase conventions. 
- Always use braces, even for single line if statements
- Braces always go on a new line
- Never prefix private variables with _ or some other prefix, e.g. m_, instead, use camelCase ```privateVariable``` and use the ```this.privateVariable``` to reference private variables inside classes. That's what the ```this``` keyword is there for.
	- Except for ```[SerializeField]``` and ```[SerializeReference]```, which are psuedo public variables and don't require ```this.```
- Always use the most specific visibility modifier for variables, properties and functions, except for standard Unity functions, which should have no modifier. e.g. ```void Update()```
- Don't use ```public``` variables, use properties
- Use ```[SerializeField] private``` (or ```protected```), not ```public``` variables for editing enabled variables.
- Interface names are prefixed with I
- Prefer typing out the full english word, rather than shortened words, unless the shortened word is common usage, e.g. max & min.
- Use the ```this.``` prefix when referencing internal MonoBehaviour member variables, such as gameObject and transform within a MonoBehaviour class
- If using Debug.Log in a MonoBehaviour/ScriptableObject, include the this reference (```Debug.Log("error message", this);``` to enable Unity to reference the source line of the debug message.

https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md