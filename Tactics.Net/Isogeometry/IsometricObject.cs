using System;
using SFML.Graphics;
using SFML.System;
using Tactics.Net.Extensions;

namespace Tactics.Net.Isogeometry
{
    //========================================================================================================================
    // ** Isometric Object (Abstract)
    //========================================================================================================================
    // Represents an object which is drawn using an isometric angle.
    //========================================================================================================================
    public abstract class IsometricObject : Disposable, Drawable
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Get Height
        //--------------------------------------------------------------------------------------------------------------------
        // * offset : (x,y) position relative to the center of the x-y axis of object
        //--------------------------------------------------------------------------------------------------------------------
        public float Height() { return Height(new Vector2f(0, 0)); }
        public virtual float Height(Vector2f offset)
        {
            return 1.0f;
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Fire Position Changed Event
        //--------------------------------------------------------------------------------------------------------------------
        protected virtual void OnPositionChange(Vector3f oldPosition, Vector3f newPosition)
        {
            GlobalPosition = Isogeometry.IsoToGlobal(Position);
            PositionChanged?.Invoke(this, new PositionChangedEventArgs() { OldPosition = oldPosition, NewPosition = newPosition, });
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Draw (Abstract, Implementation)
        //--------------------------------------------------------------------------------------------------------------------
        public abstract void Draw(RenderTarget target, RenderStates states);

        //--------------------------------------------------------------------------------------------------------------------
        // - Global Bounding Rectangle (Abstract)
        //--------------------------------------------------------------------------------------------------------------------
        public abstract FloatRect GetGlobalBounds();

        //--------------------------------------------------------------------------------------------------------------------
        // - Position (Property)
        //--------------------------------------------------------------------------------------------------------------------
        // Fires an event when the object's position is effectively changed
        //--------------------------------------------------------------------------------------------------------------------
        private Vector3f position_;
        public Vector3f Position {
            get { return position_; }
            set
            {
                if (position_ != value)
                {
                    Vector3f oldPosition = position_;
                    position_ = value;
                    OnPositionChange(oldPosition, position_);
                }
            }
        }

        // Members
        public Vector2f GlobalPosition { get; set; }

        // Members - Events
        public event EventHandler<PositionChangedEventArgs> PositionChanged;
    }

    //========================================================================================================================
    // ** Descriptor for Changing Position Event Arguments
    //========================================================================================================================
    public class PositionChangedEventArgs
    {
        public Vector3f OldPosition { get; set; }
        public Vector3f NewPosition { get; set; }
    }
}
