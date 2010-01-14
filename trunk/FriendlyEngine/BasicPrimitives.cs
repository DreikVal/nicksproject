﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FriendlyEngine
{
    public class BasicPrimitives
    {
        /*********************************************************************/
        // Members.
        /*********************************************************************/

        #region Fields

        /// <summary>The color of the primitive object.</summary>
        private Color m_Color = Color.White;

        /// <summary>The position of the primitive object.</summary>
        private Vector2 m_vPosition = Vector2.Zero;

        /// <summary>The render depth of the primitive line object (0 = front, 1 = back).</summary>
        private float m_fDepth = 0f;

        /// <summary>The thickness of the shape's edge.</summary>
        private float m_fThickness = 1f;

        /// <summary>1x1 pixel that creates the shape.</summary>
        private Texture2D m_Pixel = null;

        /// <summary>List of vectors.</summary>
        private List<Vector2> m_VectorList = new List<Vector2>();

        #endregion // Fields

        #region Properties

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get/Set the colour of the primitive object.
        /// </summary>
        //////////////////////////////////////////////////////////////////////////
        public Color Colour
        {
            get { return m_Color; }
            set { m_Color = value; }
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get/Set the position of the primitive object.
        /// </summary>
        //////////////////////////////////////////////////////////////////////////
        public Vector2 Position
        {
            get { return m_vPosition; }
            set { m_vPosition = value; }
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get/Set the render depth of the primitive line object (0 = front, 1 = back).
        /// </summary>
        //////////////////////////////////////////////////////////////////////////
        public float Depth
        {
            get { return m_fDepth; }
            set { m_fDepth = value; }
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get/Set the thickness of the shape's edge.
        /// </summary>
        //////////////////////////////////////////////////////////////////////////
        public float Thickness
        {
            get { return m_fThickness; }
            set { m_fThickness = value; }
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the number of vectors which make up the primitive object.
        /// </summary>
        //////////////////////////////////////////////////////////////////////////
        public int CountVectors
        {
            get { return m_VectorList.Count; }
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the vector position from the list.
        /// </summary>
        /// <param name="_nIndex">The index to get from.</param>
        //////////////////////////////////////////////////////////////////////////
        public Vector2 GetVector(int _nIndex)
        {
            return m_VectorList[_nIndex];
        }

        #endregion // Properties

        /*********************************************************************/
        // Functions.
        /*********************************************************************/

        #region Initialization | Dispose

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a new primitive object.
        /// </summary>
        /// <param name="_graphicsDevice">The graphics device object to use.</param>
        //////////////////////////////////////////////////////////////////////////
        public BasicPrimitives(GraphicsDevice _graphicsDevice)
        {
            //////////////////////////////////////////////////////////////////////////
            // Create the pixel texture.
            m_Pixel = new Texture2D(_graphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
            m_Pixel.SetData<Color>(new Color[] { Color.White });
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called when the primitive object is destroyed.
        /// </summary>
        //////////////////////////////////////////////////////////////////////////
        ~BasicPrimitives()
        {
            m_Pixel.Dispose();
            m_VectorList.Clear();
        }

        #endregion // Initialization | Dispose

        #region List Manipulation Methods

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds a vector to the primitive object.
        /// </summary>
        /// <param name="_vPosition">The vector to add.</param>
        //////////////////////////////////////////////////////////////////////////
        public void AddVector(Vector2 _vPosition)
        {
            m_VectorList.Add(_vPosition);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Inserts a vector into the primitive object.
        /// </summary>
        /// <param name="_nIndex">The index to insert it at.</param>
        /// <param name="_vPosition">The vector to insert.</param>
        //////////////////////////////////////////////////////////////////////////
        public void InsertVector(int _nIndex, Vector2 _vPosition)
        {
            m_VectorList.Insert(_nIndex, _vPosition);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Removes a vector from the primitive object.
        /// </summary>
        /// <param name="_vPosition">The vector to remove.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RemoveVector(Vector2 _vPosition)
        {
            m_VectorList.Remove(_vPosition);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Removes a vector from the primitive object.
        /// </summary>
        /// <param name="_nIndex">The index of the vector to remove.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RemoveVector(int _nIndex)
        {
            m_VectorList.RemoveAt(_nIndex);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clears all vectors from the list.
        /// </summary>
        //////////////////////////////////////////////////////////////////////////
        public void ClearVectors()
        {
            m_VectorList.Clear();
        }

        #endregion // List Manipulation Methods

        #region Creation Methods

        //////////////////////////////////////////////////////////////////////////
        /// <summary> 
        /// Create a line primitive.
        /// </summary>
        /// <param name="_vStart">Start of the line, in pixels.</param>
        /// <param name="_vEnd">End of the line, in pixels.</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateLine(Vector2 _vStart, Vector2 _vEnd)
        {
            m_VectorList.Clear();
            m_VectorList.Add(_vStart);
            m_VectorList.Add(_vEnd);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create a triangle primitive.
        /// </summary>
        /// <param name="_vPoint1">Fist point, in pixels.</param>
        /// <param name="_vPoint2">Second point, in pixels.</param>
        /// <param name="_vPoint3">Third point, in pixels.</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateTriangle(Vector2 _vPoint1, Vector2 _vPoint2, Vector2 _vPoint3)
        {
            m_VectorList.Clear();
            m_VectorList.Add(_vPoint1);
            m_VectorList.Add(_vPoint2);
            m_VectorList.Add(_vPoint3);
            m_VectorList.Add(_vPoint1);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create a square primitive.
        /// </summary>
        /// <param name="_vTopLeft">Top left hand corner of the square.</param>
        /// <param name="_vBottomRight">Bottom right hand corner of the square.</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateSquare(Vector2 _vTopLeft, Vector2 _vBottomRight)
        {
            m_VectorList.Clear();
            m_VectorList.Add(_vTopLeft);
            m_VectorList.Add(new Vector2(_vTopLeft.X, _vBottomRight.Y));
            m_VectorList.Add(_vBottomRight);
            m_VectorList.Add(new Vector2(_vBottomRight.X, _vTopLeft.Y));
            m_VectorList.Add(_vTopLeft);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a circle starting from (0, 0).
        /// </summary>
        /// <param name="_fRadius">The radius (half the width) of the circle.</param>
        /// <param name="_nSides">The number of sides on the circle. (64 is average).</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateCircle(float _fRadius, int _nSides)
        {
            m_VectorList.Clear();

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            float fMax = (float)MathHelper.TwoPi;
            float fStep = fMax / (float)_nSides;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Create the full circle.
            for (float fTheta = fMax; fTheta >= -1; fTheta -= fStep)
            {
                m_VectorList.Add(new Vector2(_fRadius * (float)Math.Cos((double)fTheta),
                                             _fRadius * (float)Math.Sin((double)fTheta)));
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an ellipse starting from (0, 0) with the given width and height.
        /// Vectors are generated using the parametric equation of an ellipse.
        /// </summary>
        /// <param name="_fSemiMajorAxis">The width of the ellipse at its center.</param>
        /// <param name="_fSemiMinorAxis">The height of the ellipse at its center.</param>
        /// <param name="_nSides">The number of sides on the ellipse. (64 is average).</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateEllipse(float _fSemiMajorAxis, float _fSemiMinorAxis, int _nSides)
        {
            m_VectorList.Clear();

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            float fMax = (float)MathHelper.TwoPi;
            float fStep = fMax / (float)_nSides;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Create full ellipse.
            for (float fTheta = fMax; fTheta >= -1; fTheta -= fStep)
            {
                m_VectorList.Add(new Vector2((float)(_fSemiMajorAxis * Math.Cos(fTheta)),
                                             (float)(_fSemiMinorAxis * Math.Sin(fTheta))));
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        #endregion // Creation Methods

        #region Render Methods

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render points of the primitive.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderPointPrimitive(SpriteBatch _spriteBatch)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count <= 0)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero;
            float fAngle = 0f;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Stretch the pixel between the two vectors.
                _spriteBatch.Draw(m_Pixel,
                                  m_vPosition + m_VectorList[i],
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0.5f, 0.5f),
                                  m_fThickness,
                                  SpriteEffects.None,
                                  m_fDepth);
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render points of the primitive.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        /// <param name="_fAngle">The counterclockwise rotation in radians. (0.0f is default).</param>
        /// <param name="_vPivot">Position in which to rotate around.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderPointPrimitive(SpriteBatch _spriteBatch, float _fAngle, Vector2 _vPivot)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count <= 0)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Rotate object based on pivot.
            Rotate(_fAngle, _vPivot);
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero;
            float fAngle = 0f;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Stretch the pixel between the two vectors.
                _spriteBatch.Draw(m_Pixel,
                                  m_vPosition + m_VectorList[i],
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0.5f, 0.5f),
                                  m_fThickness,
                                  SpriteEffects.None,
                                  m_fDepth);
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render the lines of the primitive.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderLinePrimitive(SpriteBatch _spriteBatch)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];

                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Stretch the pixel between the two vectors.
                _spriteBatch.Draw(m_Pixel,
                                  m_vPosition + vPosition1,
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0, 0.5f),
                                  new Vector2(fDistance, m_fThickness),
                                  SpriteEffects.None,
                                  m_fDepth);
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render the lines of the primitive.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        /// <param name="_fAngle">The counterclockwise rotation in radians. (0.0f is default).</param>
        /// <param name="_vPivot">Position in which to rotate around.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderLinePrimitive(SpriteBatch _spriteBatch, float _fAngle, Vector2 _vPivot)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Rotate object based on pivot.
            Rotate(_fAngle, _vPivot);
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];

                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Stretch the pixel between the two vectors.
                _spriteBatch.Draw(m_Pixel,
                                  m_vPosition + vPosition1,
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0, 0.5f),
                                  new Vector2(fDistance, m_fThickness),
                                  SpriteEffects.None,
                                  m_fDepth);
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render primitive by using a square algorithm.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderSquarePrimitive(SpriteBatch _spriteBatch)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero, vLength = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            int nCount = 0;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                //////////////////////////////////////////////////////////////////////////
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Calculate length.
                vLength = vPosition2 - vPosition1;
                vLength.Normalize();

                // Calculate count for roundness.
                nCount = (int)Math.Round(fDistance);
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Run through and render the primitive.
                while (nCount-- > 0)
                {
                    // Increment position.
                    vPosition1 += vLength;

                    // Stretch the pixel between the two vectors.
                    _spriteBatch.Draw(m_Pixel,
                                      m_vPosition + vPosition1,
                                      null,
                                      m_Color,
                                      0,
                                      Vector2.Zero,
                                      m_fThickness,
                                      SpriteEffects.None,
                                      m_fDepth);
                }
                //
                //////////////////////////////////////////////////////////////////////////
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render primitive by using a square algorithm.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        /// <param name="_fAngle">The counterclockwise rotation in radians. (0.0f is default).</param>
        /// <param name="_vPivot">Position in which to rotate around.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderSquarePrimitive(SpriteBatch _spriteBatch, float _fAngle, Vector2 _vPivot)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Rotate object based on pivot.
            Rotate(_fAngle, _vPivot);
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero, vLength = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            int nCount = 0;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                //////////////////////////////////////////////////////////////////////////
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Calculate length.
                vLength = vPosition2 - vPosition1;
                vLength.Normalize();

                // Calculate count for roundness.
                nCount = (int)Math.Round(fDistance);
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Run through and render the primitive.
                while (nCount-- > 0)
                {
                    // Increment position.
                    vPosition1 += vLength;

                    // Stretch the pixel between the two vectors.
                    _spriteBatch.Draw(m_Pixel,
                                      m_vPosition + vPosition1,
                                      null,
                                      m_Color,
                                      0,
                                      Vector2.Zero,
                                      m_fThickness,
                                      SpriteEffects.None,
                                      m_fDepth);
                }
                //
                //////////////////////////////////////////////////////////////////////////
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render primitive by using a round algorithm.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderRoundPrimitive(SpriteBatch _spriteBatch)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero, vLength = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            int nCount = 0;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                //////////////////////////////////////////////////////////////////////////
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Calculate length.
                vLength = vPosition2 - vPosition1;
                vLength.Normalize();

                // Calculate count for roundness.
                nCount = (int)Math.Round(fDistance);
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Run through and render the primitive.
                while (nCount-- > 0)
                {
                    // Increment position.
                    vPosition1 += vLength;

                    // Stretch the pixel between the two vectors.
                    _spriteBatch.Draw(m_Pixel,
                                      m_vPosition + vPosition1 + 0.5f * (vPosition2 - vPosition1),
                                      null,
                                      m_Color,
                                      fAngle,
                                      new Vector2(0.5f, 0.5f),
                                      new Vector2(fDistance, m_fThickness),
                                      SpriteEffects.None,
                                      m_fDepth);
                }
                //
                //////////////////////////////////////////////////////////////////////////
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render primitive by using a round algorithm.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        /// <param name="_fAngle">The counterclockwise rotation in radians. (0.0f is default).</param>
        /// <param name="_vPivot">Position in which to rotate around.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderRoundPrimitive(SpriteBatch _spriteBatch, float _fAngle, Vector2 _vPivot)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Rotate object based on pivot.
            Rotate(_fAngle, _vPivot);
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero, vLength = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            int nCount = 0;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                //////////////////////////////////////////////////////////////////////////
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Calculate length.
                vLength = vPosition2 - vPosition1;
                vLength.Normalize();

                // Calculate count for roundness.
                nCount = (int)Math.Round(fDistance);
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Run through and render the primitive.
                while (nCount-- > 0)
                {
                    // Increment position.
                    vPosition1 += vLength;

                    // Stretch the pixel between the two vectors.
                    _spriteBatch.Draw(m_Pixel,
                                      m_vPosition + vPosition1 + 0.5f * (vPosition2 - vPosition1),
                                      null,
                                      m_Color,
                                      fAngle,
                                      new Vector2(0.5f, 0.5f),
                                      new Vector2(fDistance, m_fThickness),
                                      SpriteEffects.None,
                                      m_fDepth);
                }
                //
                //////////////////////////////////////////////////////////////////////////
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render primitive by using a point and line algorithm.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderPolygonPrimitive(SpriteBatch _spriteBatch)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                //////////////////////////////////////////////////////////////////////////
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Stretch the pixel between the two vectors.
                _spriteBatch.Draw(m_Pixel,
                                  Position + vPosition1 + 0.5f * (vPosition2 - vPosition1),
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0.5f, 0.5f),
                                  new Vector2(fDistance, Thickness),
                                  SpriteEffects.None,
                                  m_fDepth);

                // Render the points of the polygon.
                _spriteBatch.Draw(m_Pixel,
                                  m_vPosition + vPosition1,
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0.5f, 0.5f),
                                  m_fThickness,
                                  SpriteEffects.None,
                                  m_fDepth);
                //
                //////////////////////////////////////////////////////////////////////////
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render primitive by using a point and line algorithm.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        /// <param name="_fAngle">The counterclockwise rotation in radians. (0.0f is default).</param>
        /// <param name="_vPivot">Position in which to rotate around.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderPolygonPrimitive(SpriteBatch _spriteBatch, float _fAngle, Vector2 _vPivot)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Rotate object based on pivot.
            Rotate(_fAngle, _vPivot);
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                //////////////////////////////////////////////////////////////////////////
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Stretch the pixel between the two vectors.
                _spriteBatch.Draw(m_Pixel,
                                  Position + vPosition1 + 0.5f * (vPosition2 - vPosition1),
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0.5f, 0.5f),
                                  new Vector2(fDistance, Thickness),
                                  SpriteEffects.None,
                                  m_fDepth);

                // Render the points of the polygon.
                _spriteBatch.Draw(m_Pixel,
                                  m_vPosition + vPosition1,
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0.5f, 0.5f),
                                  m_fThickness,
                                  SpriteEffects.None,
                                  m_fDepth);
                //
                //////////////////////////////////////////////////////////////////////////
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        #endregion // Render Methods

        #region Public Methods

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Rotate primitive object based on pivot.
        /// </summary>
        /// <param name="_fAngle">The counterclockwise rotation in radians. (0.0f is default).</param>
        /// <param name="_vPivot">Position in which to rotate around.</param>
        //////////////////////////////////////////////////////////////////////////
        public void Rotate(float _fAngle, Vector2 _vPivot)
        {
            //////////////////////////////////////////////////////////////////////////
            // Subtract pivot from all points.
            for (int i = m_VectorList.Count - 1; i >= 0; --i)
                m_VectorList[i] -= _vPivot;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Rotate about the origin.
            Matrix mat = Matrix.CreateRotationZ(_fAngle);
            for (int i = m_VectorList.Count - 1; i >= 0; --i)
                m_VectorList[i] = Vector2.Transform(m_VectorList[i], mat);
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Add pivot to all points.
            for (int i = m_VectorList.Count - 1; i >= 0; --i)
                m_VectorList[i] += _vPivot;
            //
            //////////////////////////////////////////////////////////////////////////
        }

        #endregion // Public Methods
    }
}
