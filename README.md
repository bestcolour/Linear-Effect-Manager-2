# LinearEffectManager-2
 
Hi there! Linear Effects Manager 2 is a iteration of my first project: Linear Event Manager (right... i forgot i changed the naming of the files from Event to Effect)

This project is purely written in C# and is more compact, efficient and customizeable than the first version!

You can import this file as a template for a Unity project or switch to the "package" branch to have the plucked out files of the tool!

<img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/Title.jpg"></img>


<h2>How to Start</h2>
Create a gameobject and add FlowChart or BaseFlowChart component onto it. 

<h2>Hide Executors</h2>
When your FlowChart gets complicated, there will be a lot of executor components on your gameobject. You can hide it with the hide executor toggle.
<img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/HideExecutors.gif"></img>

<h2>Creating A Block & Adding Effects</h2>
After opening the flowchart, a node editor window will open. In here, you can start off by creating a block by either pressing the "+" symbol on the top left corner or right clicking on the graph and selecting "New Block". A block is like a box that holds a list of effects which will be called in a sequential order. When your block has been created, click on the block view it in the inspector. The information on the inspector is about the current selected block. You can add effects and re-order them however you want here.
<img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/CreatingNewBlock.gif"></img>

<h2>Copy Pasting Effects</h2>
You can copy paste effects within the block inspector. Simply click once and then shift click to multi-select effects which you want to copy and paste, then press the copy and paste buttons to duplicate the effects.
<img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/CopyPastingEffect.gif"></img>


<h2>Deleting Effects</h2>
You can delete effects within the block inspector. Simply click once and then shift click to multi-select effects which you want to delete, then press the delete button.
<img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/DeletingEffects.gif"></img>

<h2>Duplicating Blocks</h2>
You can duplicate blocks in the flowchart. Simply select the block(s) which you want to duplicate and press the duplicate on the top left corner or simply right click on the graph and select "duplicate".
<img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/DuplicatingBlocks.gif"></img>

<h2>Arrow Mode</h2>
You can draw arrows within the node editor (though they are only visually for show and do not actually play the connected blocks). To do so, select a block and press on the Arrow Mode button on the top right corner of the flowchart. Next, select the block which you want the arrow to go towards. To exit the Arrow mode, click on the Exit button on the top right corner of the flowchart or right click on the graph and click "Exit Arrow Mode".
<img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/ArrowMode.gif"></img>

