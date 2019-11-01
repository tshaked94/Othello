using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05_Othello.Logic
{
    public abstract class Player
    {
        public enum eColor
        {
            Yellow = '0',
            Red = '1'
        }
        
        protected int m_CurrentRoundPlayerScore;
        protected int m_OverallPlayerScore = 0;
        protected eColor m_PlayerColor;
         
        public virtual eColor Color
        {
            // a propertie for m_CurrentRoundPlayerScore
            get
            {
                
                return m_PlayerColor;
            }

            set
            {
                m_PlayerColor = value;
            }
        }

        public virtual int OverallScore
        {
            // a propertie for m_OverallPlayerScore
            get
            {

                return m_OverallPlayerScore;
            }

            set
            {
                m_OverallPlayerScore = value;
            }
        }

        public virtual int RoundScore
        {
            // a propertie for m_PlayerScore
            get
            {

                return m_CurrentRoundPlayerScore;
            }

            set
            {
                m_CurrentRoundPlayerScore = value;
            }
        }
    }
}
