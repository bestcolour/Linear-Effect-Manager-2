# LinearEffectManager-2
 
Hi there! Linear Effects Manager 2 is a iteration of my first project: Linear Event Manager (right... i forgot i changed the naming of the files from Event to Effect)

This project is purely written in C# and is more compact, efficient and customizeable than the first version!

You can import this file as a template for a Unity project or switch to the "package" branch to have the plucked out files of the tool!

<img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/Title.jpg"></img>


### Content Page
1. [Basic Controls](#basic-controls)	
			<br/>a) [How to start](#how-to-start) 
			<br/>b) [Hiding Executors](#hide-executors) 
			<br/>c) [Creating A Block & Adding Effects](#creating-a-block-and-adding-effects) 
			<br/>d) [Copy Pasting Effects](#copy-pasting-effects) 
			<br/>e) [Deleting Effects](#deleting-effects) 
			<br/>f) [Duplicating Blocks](#duplicating-blocks) 
			<br/>g) [Arrow Mode](#arrow-mode) 



2. [Adding Custom Effects](#adding-custom-effects)




### Basic Controls
#### How To Start
Create a gameobject and add FlowChart or BaseFlowChart component onto it. 

#### Hide Executors
When your FlowChart gets complicated, there will be a lot of executor components on your gameobject. You can hide it with the hide executor toggle.
<br/><img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/HideExecutors.gif"></img>

#### Creating A Block and Adding Effects
After opening the flowchart, a node editor window will open. In here, you can start off by creating a block by either pressing the "+" symbol on the top left corner or right clicking on the graph and selecting "New Block". A block is like a box that holds a list of effects which will be called in a sequential order. When your block has been created, click on the block view it in the inspector. The information on the inspector is about the current selected block. You can add effects and re-order them however you want here.
<br/><img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/CreatingNewBlock.gif"></img>

#### Copy Pasting Effects
You can copy paste effects within the block inspector. Simply click once and then shift click to multi-select effects which you want to copy and paste, then press the copy and paste buttons to duplicate the effects.
<br/><img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/CopyPastingEffect.gif"></img>


#### Deleting Effects
You can delete effects within the block inspector. Simply click once and then shift click to multi-select effects which you want to delete, then press the delete button.
<br/><img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/DeletingEffects.gif"></img>

#### Duplicating Blocks
You can duplicate blocks in the flowchart. Simply select the block(s) which you want to duplicate and press the duplicate on the top left corner or simply right click on the graph and select "duplicate".
<br/><img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/DuplicatingBlocks.gif"></img>

#### Arrow Mode
You can draw arrows within the node editor (though they are only visually for show and do not actually play the connected blocks). To do so, select a block and press on the Arrow Mode button on the top right corner of the flowchart. Next, select the block which you want the arrow to go towards. To exit the Arrow mode, click on the Exit button on the top right corner of the flowchart or right click on the graph and click "Exit Arrow Mode".
<br/><img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/ArrowMode.gif"></img>




### Adding Custom Effects


