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
			<br/>d) [UpdateEffectExecutor Introduction](#updateEffectExecutor-introduction) 
			<br/>e) [UpdateEffectExecutor Guide](#updateEffectExecutor-guide) 
			<br/>f) [Important rules to adhere to when adding to EffectsData.cs](#important-rules-to-adhere-to-when-adding-to-effectsData.cs) 



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
    [System.Serializable]
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

4. Inherit from the "EffectExecutor" class. Since we are <b>not</b> creating an executor which calls its code over multiple frames (aka the update executor), inherit from the "Effect" class. I like to keep things organized so I declared my class inside of my executor. Remember to add the "System.Serializable" attribute to your custom effect class.



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
    [System.Serializable]
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


<br/>
<br/>
<br/>
<br/>

Inside of EffectsData.cs
```
	   static readonly Dictionary<string, Type> ExecutorLabel_To_EffectExecutor = new Dictionary<string, Type>()
	{
	    //=================== EXAMPLE ===================
            //{"Custom/Lalalala/CustomEffectExample_Executor", typeof(<INSERT EXECUTOR NAME HERE>)},
            //        ^          		^                                ^
            //        A            		B                                C
            //
            //A: How you want to catergorise your executors, use forwardslash to create a folder
            //B: What you want your Executor to be named when it shows up in the search box (you cannot have duplicate names)
            //C: Your Executor type, reminder to not rename this once you started using it in Blocks
	    ...
	}

```

6. Finally, we need to add your new custom executor into the EffectData.cs script (which is found in the Editor Folder inside of the LEM2_Scripts folder) so that it can be found in the searchbox on the block inspector. To do so, we have to add a new key-value pair to the ExecutorLabel_To_EffectExecutor dictionary found inside of EffectData.cs. Above is an example of how you can name your custom effects.

<br/>
<br/>
<br/>



### UpdateEffectExecutor Introduction
```   
//The UpdateEffectExecutor is assumed to have an ExecuteEffect function which needs multiple frame to complete
    ///<Summary>A base effectexecutor which will finish its effect in a more than single frame</Summary>
    public abstract class UpdateEffectExecutor<T> : EffectExecutor<T>
    where T : UpdateEffect, new()
```
Inherit from this class if you wish to have a custom effect which requires multiple frames to complete (eg. Fading a UI element). T is a generic type which can be instantiated with the "new" keyword and inherits from the LinearEffects.UpdateEffect class.

<br/>
<br/>
<br/>


### UpdateEffectExecutor Guide
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
public class CustomEffectExample_Executor : UpdateEffectExecutor<>
{

}
```

3. Inherit from the "UpdateEffectExecutor" class. 


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
public class CustomEffectExample_Executor : UpdateEffectExecutor<CustomEffectExample_Executor.MyEffect>
{
    //You have to create a class which inherits from either the Effect or UpdateEffect class
    //The decision for the type of class to inherit from is from the type of effect executor you are creating.
    public class MyEffect : UpdateEffect
    {
        //Declare whatever variables & methods you want to use in your effect
        [SerializeField]
        int _countBeforeTimeUp = 5;
	
	//Runtime
	int _currentCount = 0;

	public void Reset()
	{
	    _currentCount = 0;
	}

        public bool UpdateCount()
        {
	    _currentCount++;
	    if(_currentCount >= _countBeforeTimeUp)
	    {
	    	//Return true when effect is completed
	    	return true;
	    }
	    return false;
        }

    }
}
```

4. Inherit from the "UpdateEffectExecutor" class. Since we are creating an executor which calls its code over multiple frames (aka the update executor), inherit from the "UpdateEffect" class. I like to keep things organized so I declared my class inside of my executor.



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
public class CustomEffectExample_Executor : UpdateEffectExecutor<CustomEffectExample_Executor.MyEffect>
{
    //You have to create a class which inherits from either the Effect or UpdateEffect class
    //The decision for the type of class to inherit from is from the type of effect executor you are creating.
    public class MyEffect : UpdateEffect
    {
  	//Declare whatever variables & methods you want to use in your effect
        [SerializeField]
        int _countBeforeTimeUp = 5;
	
	//Runtime
	int _currentCount = 0;

	public void Reset()
	{
	    _currentCount = 0;
	}

        public bool UpdateCount()
        {
	    _currentCount++;
	    if(_currentCount >= _countBeforeTimeUp)
	    {
	    	//Return true when effect is completed
	    	return true;
	    }
	    return false;
        }

    }
    
    
	//Overide this method if you wish to call something before the effect starts updating.
       ///<Summary>The method called before calling ExecuteEffect method. Then number of times this method is called depends on the number of times you decide to play the block this effect is on. Add your reset methods here for your effect classes.</Summary>
        protected override void BeginExecuteEffect(T effectData)
        {
            base.BeginExecuteEffect(effectData);
	    effectData.Reset();
	    //Add what you want to do before the effect starts updating.
        }

	//Overide this method if you wish to call something when the effect finishes updating.
        ///<Summary>The method called when ExecuteEffect method is finally finished updating. (The frame in which ExecuteEffect returns true)</Summary>
        protected virtual void EndExecuteEffect(T effectData)
        {
            base.EndExecuteEffect(effectData);
	    Debug.Log("Ended!")
	    //Add what you want to do when the effect has finished updating.
        }
    
     //This method can be auto generated (if you are using visual studio code or visual studio, there is a shortcut key for it)
    //but basically, add whatever you want to execute here to "Execute your effect" 
    //and always ensure to return true if your effect is completed
    //This will be called everyframe when your effect is being called.
    protected override bool ExecuteEffect(MyEffect effectData)
    {
	return effectData.UpdateCount();
    }

}
```

5. Override the ExecuteEffect(MyEffect effectData) and do whatever you want inside this method to execute your effect.


<br/>
<br/>
<br/>
<br/>

Inside of EffectsData.cs
``` 
        static readonly Dictionary<string, Type> ExecutorLabel_To_EffectExecutor = new Dictionary<string, Type>()
	{
	    //=================== EXAMPLE ===================
            //{"Example/CustomEffectExample_Executor", typeof(<INSERT EXECUTOR NAME HERE>)},
            //    ^            ^                                ^
            //    A            B                                C
            //
            //A: How you want to catergorise your executors, use forwardslash to create a folder
            //B: What you want your Executor to be named when it shows up in the search box (you cannot have duplicate names)
            //C: Your Executor type, reminder to not rename this once you started using it in Blocks
	    ...
	}

```

6. Finally, we need to add your new custom executor into the EffectData.cs script (which is found in the Editor Folder inside of the LEM2_Scripts folder) so that it can be found in the searchbox on the block inspector. To do so, we have to add a new key-value pair to the ExecutorLabel_To_EffectExecutor dictionary found inside of EffectData.cs. Above is an example of how you can name your custom effects.

<br/>
<br/>
<br/>






### Important rules to adhere to when adding to EffectsData.cs
<br/>
Dictionary key: The path of the effect executor to be shown in the SearchBox. There are two parts to the key: FullExecutorName and ExecutorName.
	- The FullExecutorName is the entire string path with all the slashes. The anything inbetween the start of the string path to the start of the ExecutorName can be changed freely
	- The ExecutorName is whatever you call the Executor at the end of the last slash. Please note that there cannot be duplicate ExecutorName in the Dictionary. The ExecutorName can be named differently from the Executor Type's name (which is the Dictionary's Value) but should not be renamed after the executor has been used.

<br/>
Dictionary Value: The System type of your own custom effect executor. Do not rename this once you started using it in Blocks.

	

