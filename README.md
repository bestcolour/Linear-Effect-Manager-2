# LinearEffectManager-2
 
Hi there! Linear Effects Manager 2 is a iteration of my first project: Linear Event Manager (right... i forgot i changed the naming of the files from Event to Effect)

This project is purely written in C# and is more compact, efficient and customizeable than the first version!

Go to the "package" branch to get only the package or main to get the template Unity project folder!

This is a branch where there are events scriptableobjects and their respective executors. The scriptableobjects are used to replace the UnityEvent in the Effect List of the Block Inspector as there is a bug where removing a UnityEvent on a block will cause the other blocks with UnityEvents on it to duplicate the removed UnityEvent's values.

It is kept separate from the main branch as there are dependencies added to this branch. 


