using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Tactics.Net.Sprites.Actor
{
    //========================================================================================================================
    // ** Isometric Character Sprite
    //========================================================================================================================
    // Defines a set of basic animations universal to all actors, including NPCs, Enemies and Playable Characters such as
    // walking or standing; also contains a customizable dictionary of special named animations sequences
    //========================================================================================================================
    public class SpriteActor : SpriteAnimated
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Character Sprite Constructror
        //--------------------------------------------------------------------------------------------------------------------
        public SpriteActor(Texture texture, uint width = 0, uint height = 0, uint pages = 4, float framerate = 60)
            : base(texture, width, height, pages, framerate)
        {
            // Fill all basic sequences with a single frame
            BasicSequences = Enumerable.Repeat(new List<uint>{0}, Enum.GetValues(typeof(BasicAnimations)).Length).ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Play Animation Sequence (Basic)
        //--------------------------------------------------------------------------------------------------------------------
        public void Play(BasicAnimations animation, bool loop = false)
        {
            Sequence = BasicSequences[(int)animation];
            Play(loop);
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Play Animation Sequence (Basic, Specified Duration)
        //--------------------------------------------------------------------------------------------------------------------
        public void Play(BasicAnimations animation, float duration)
        {
            Sequence = BasicSequences[(int)animation];
            Play(duration);
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Set Basic Animation Sequence
        //--------------------------------------------------------------------------------------------------------------------
        public void SetSequence(BasicAnimations animation, List<uint> sequence)
        {
            if(sequence.Any())
            {
                // If the currently playing animation is being replaced, set the new sequence as the current active and Stop
                if(Sequence == BasicSequences[(int)animation])
                {
                    Sequence = sequence;
                }
                BasicSequences[(int)animation] = sequence;
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // Play Animation Sequence (Named)
        //--------------------------------------------------------------------------------------------------------------------
        public void Play(string sequenceName, bool loop = false)
        {
            if (Sequences.ContainsKey(sequenceName))
            {
                Sequence = Sequences[sequenceName];
                Play(loop);
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // Play Animation Sequence (Named, Specified Duration)
        //--------------------------------------------------------------------------------------------------------------------
        public void Play(string sequenceName, float duration)
        {
            if (Sequences.ContainsKey(sequenceName))
            {
                Sequence = Sequences[sequenceName];
                Play(duration);
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Add/Set Named Animation Sequence
        //--------------------------------------------------------------------------------------------------------------------
        public void AddSequence(string name, List<uint> sequence)
        {
            if(sequence.Any())
            {
                // If the currently playing animation is being replaced, set the new sequence as the current active and Stop
                if (Sequences.ContainsKey(name) && Sequence == Sequences[name])
                {
                    Sequence = sequence;
                }
                Sequences[name] = sequence;
            }
        }        

        // Members - private
        protected List<uint>[] BasicSequences { get; }
        protected Dictionary<string, List<uint>> Sequences { get; } = new Dictionary<string, List<uint>>();

        //====================================================================================================================
        // * Universal Basic Action Sequences
        //====================================================================================================================
        public enum BasicAnimations
        {
            Stand,
            Walk,
            Attack,
            Cheer,
            Wounded,
            Damage,
            Die,
        };
    }
}
