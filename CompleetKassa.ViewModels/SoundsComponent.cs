using CompleetKassa.ViewModels.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleetKassa.ViewModels
{
    class SoundsComponent
    {
        
        public void Error()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Resources.Access_denied__5_);
            player.Play();
        }
        public void Save()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Resources.Save__16_);
            player.Play();
        }
        public void Numpad()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Resources.Numpad__6_);
            player.Play();
        }
        public void NumpadOpen()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Resources.go_to_numpad__7_);
            player.Play();
        }

        public void Press()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Resources.buttonpress_Sounds__13_);
            player.Play();
        }

        public void Delete()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Resources.delete);
            player.Play();
        }
        public void Products()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Resources.add_product__5_);
            player.Play();
        }
    }
}
