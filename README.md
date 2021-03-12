# LinearEffectManager-2
 
Hi there! Linear Effects Manager 2 is a iteration of my first project: Linear Event Manager (right... i forgot i changed the naming of the files from Event to Effect)

This project is purely written in C# and is more compact, efficient and customizeable than the first version!

You can go to the:
	- "master" branch for a Unity project to import from
	- "package" branch to have the plucked out files of the tool to import
	


<img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/Title.jpg"></img>


## Content Page
1. [Basic Controls](#basic-controls)	
			<br/>a) [How to start](#how-to-start) 
			<br/>b) [Hiding Executors](#hide-executors) 
			<br/>c) [Creating A Block & Adding Effects](#creating-a-block-and-adding-effects) 
			<br/>d) [Copy Pasting Effects](#copy-pasting-effects) 
			<br/>e) [Deleting Effects](#deleting-effects) 
			<br/>f) [Duplicating Blocks](#duplicating-blocks) 
			<br/>g) [Arrow Mode](#arrow-mode) 



2. [Adding Custom Effects](#adding-custom-effects)
			<br/>a) [Creating Custom Executor](#creating-custom-executor) 
			<br/>b) [EffectExecutor Introduction](#effectExecutor-introduction) 
			<br/>c) [EffectExecutor Guide](#effectExecutor-guide) 



## Basic Controls
### How To Start
Create a gameobject and add FlowChart or BaseFlowChart component onto it. 

### Hide Executors
When your FlowChart gets complicated, there will be a lot of executor components on your gameobject. You can hide it with the hide executor toggle.
<br/><img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/HideExecutors.gif"></img>

### Creating A Block and Adding Effects
After opening the flowchart, a node editor window will open. In here, you can start off by creating a block by either pressing the "+" symbol on the top left corner or right clicking on the graph and selecting "New Block". A block is like a box that holds a list of effects which will be called in a sequential order. When your block has been created, click on the block view it in the inspector. The information on the inspector is about the current selected block. You can add effects and re-order them however you want here.
<br/><img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/CreatingNewBlock.gif"></img>

### Copy Pasting Effects
You can copy paste effects within the block inspector. Simply click once and then shift click to multi-select effects which you want to copy and paste, then press the copy and paste buttons to duplicate the effects.
<br/><img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/CopyPastingEffect.gif"></img>


### Deleting Effects
You can delete effects within the block inspector. Simply click once and then shift click to multi-select effects which you want to delete, then press the delete button.
<br/><img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/DeletingEffects.gif"></img>

### Duplicating Blocks
You can duplicate blocks in the flowchart. Simply select the block(s) which you want to duplicate and press the duplicate on the top left corner or simply right click on the graph and select "duplicate".
<br/><img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/DuplicatingBlocks.gif"></img>

### Arrow Mode
You can draw arrows within the node editor (though they are only visually for show and do not actually play the connected blocks). To do so, select a block and press on the Arrow Mode button on the top right corner of the flowchart. Next, select the block which you want the arrow to go towards. To exit the Arrow mode, click on the Exit button on the top right corner of the flowchart or right click on the graph and click "Exit Arrow Mode".
<br/><img src="https://raw.githubusercontent.com/bestcolour/site/master/public/Resources/LEM/ArrowMode.gif"></img>




## Adding Custom Effects

### Creating Custom Executor
To create your custom effect, you must first create your own custom executor. An executor is basically a component which takes in data (that you input in the block inspector) and then does something with that data to accomplish an intended outcome.

<br/>
```
//In order to access the class type, you need to write use this namespace
using LinearEffects;
```
<br/>

The first step would be to figure out what kind of effect you are heading for. Is your effect going to be called and completed in one-frame or is it going to be called over multiple frames? 

### EffectExecutor Introduction
```   
///<Summary>A base effectexecutor which will finish its effect in a single frame</Summary>
    public abstract partial class EffectExecutor<T> : BaseEffectExecutor
    where T : Effect, new()
```
Inherit from this class if you wish to have a custom effect which completes in a single frame. T is a generic type which can be instantiated with the "new" keyword and inherits from the LinearEffects.Effect class.

<br/>
<br/>
<br/>

### EffectExecutor Guide
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In order to access the class type, you need to write use this namespace
using LinearEffects;
public class CustomEffectExample_Executor : Monobehaviour
{

}
```

1. Create a new C# script.
2. Add the "using LinearEffects;" namespace on top.

<br/>
<br/>
<br/>
<br/>

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In order to access the class type, you need to write use this namespace
using LinearEffects;
public class CustomEffectExample_Executor : EffectExecutor<>
{

}
```

3. Inherit from the "EffectExecutor" class. 


<br/>
<br/>
<br/>
<br/>

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In order to access the class type, you need to write use this namespace
using LinearEffects;
public class CustomEffectExample_Executor : EffectExecutor<CustomEffectExample_Executor.MyEffect>
{
    //You have to create a class which inherits from either the Effect or UpdateEffect class
    //The decision for the type of class to inherit from is from the type of effect executor you are creating.
    public class MyEffect : Effect
    {
        //Declare whatever variables & methods you want to use in your effect
        [SerializeField]
        string _message = "Hello world";

        public void ExecuteEffect()
        {
            Debug.Log(_message);
        }

    }
}
```

4. Inherit from the "EffectExecutor" class. Since we are **not** creating an executor which calls its code over multiple frames (aka the update executor), inherit from the "Effect" class. I like to keep things organized so I declared my class inside of my executor.



<br/>
<br/>
<br/>
<br/>

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In order to access the class type, you need to write use this namespace
using LinearEffects;
public class CustomEffectExample_Executor : EffectExecutor<CustomEffectExample_Executor.MyEffect>
{
    //You have to create a class which inherits from either the Effect or UpdateEffect class
    //The decision for the type of class to inherit from is from the type of effect executor you are creating.
    public class MyEffect : Effect
    {
        //Declare whatever variables & methods you want to use in your effect
        [SerializeField]
        string _message = "Hello world";

        public void ExecuteEffect()
        {
            Debug.Log(_message);
        }

    }
    
    
     //This method can be auto generated (if you are using visual studio code or visual studio, there is a shortcut key for it)
    //but basically, add whatever you want to execute here to "Execute your effect" 
    //and always ensure to return true if your effect is completed
    protected override bool ExecuteEffect(MyEffect effectData)
    {
        effectData.ExecuteEffect();
        return true;
    }

}
```

5. Override the ExecuteEffect(MyEffect effectData) and do whatever you want inside this method to execute your effect.
