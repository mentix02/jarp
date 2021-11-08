# Jarp

A simple JSON parser written for C#. 

### Why? 

Doesn't .NET already have `System.Text.Json`? Yes, it does.

But I was free and wanted to learn C# and figured writing a JSON parser would be an easy enough project to complete.
Besides, `System.Text.Json` doesn't provide a way to deserialize a JSON string to native C# objects. I understand it would
completely invalidate type safety but I wanted to use something like Python's `json.loads`.

Thus I had to resort to use `object` as the return type for `Jarp.Parse()`. Digusting, I know.
Which brings me to another point - 

<h3 align="center"><b>the code is probably horribly broken and idiotic</b></h3>
<br/>

No excuse for it other than me being a complete C# newbie. But I'll try to improve as much as possible.

I'm currently trying to parallelize the lexer using threads - should be fairly straightforward since the 
tokenization of a string is not position dependant whatsoever and the input string can sensibly be split into ~equal chunks
before the actual lexing.
