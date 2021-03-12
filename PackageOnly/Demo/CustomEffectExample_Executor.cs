using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In order to access the class type, you need to write use this namespace
using LinearEffects;

//Step 1)
//You have to decide what kind of code you are going to write

//If your code can be executed in one frame call:
//- use the EffectExecutor class
//Else if it requires updating over multiple frames, (for eg fading a text over x number of seconds)
//- use the UpdateEffectExecutor class

public class CustomEffectExample_Executor : EffectExecutor<CustomEffectExample_Executor.CustomEffectData>
{

    //Step 2)
    //You have to create a class which inherits from either the Effect or UpdateEffect class
    //The decision for the type of class to inherit from is the same as Step 1
    public class CustomEffectData : Effect
    {
        //Step 3
        //Declare whatever variables & methods you want to use in your effect
        [SerializeField]
        string _message = "Hello world";

        public void ExecuteEffect()
        {
            Debug.Log(_message);
        }

    }

    //Step 4)
    //This method can be auto generated (if you are using visual studio code or visual studio, there is a shortcut key for it)
    //but basically, add whatever you want to execute here to "Execute your effect" 
    //and always ensure to return true if your effect is completed
    protected override bool ExecuteEffect(CustomEffectData effectData)
    {
        effectData.ExecuteEffect();
        return true;
    }


    //Step 5)
    //The class is not added to the Effects library yet so when you try to search for it in the search box of the flowchart, it wont show up.
    //To do that, you have to go to the cs file "EffectsData.cs" at line 47
    //After adding the entry in the EffectsData.cs, try adding your custom effect into a block and play the game to see the console log!
}
