using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05_Othello.Logic
{
    public struct Direction
    {
        // this struct unite two dimension direction.
        public enum eDirection
        {
            Up = -1,
            Down = 1,
            Left = -1,
            Right = 1,
            NoDirection = 0
        }

        private int m_Horizontal;
        private int m_Vertical;

        public Direction(int i_Vertical, int i_Horizontal)
        {
            // Direction c'tor
            m_Vertical = i_Vertical;
            m_Horizontal = i_Horizontal;
        }

        public int Horizontal
        {
            // a propertie for m_Horizontal
            get
            {

                return m_Horizontal;
            }

            set
            {
                m_Horizontal = value;
            }
        }

        public int Vertical
        {
            // a propertie for m_Vertical
            get
            {

                return m_Vertical;
            }

            set
            {
                m_Vertical = value;
            }
        }
    }
}
