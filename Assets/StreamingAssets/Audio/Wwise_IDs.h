/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_AMBIENT = 1562304622U;
        static const AkUniqueID PLAY_BREATHING_LP = 1695191155U;
        static const AkUniqueID PLAY_CLOTHES_LP = 4119826805U;
        static const AkUniqueID PLAY_CREDITS_MUSIC = 3825865420U;
        static const AkUniqueID PLAY_HEALTHSTATE = 2456347459U;
        static const AkUniqueID PLAY_INGAME_MUSIC = 347845317U;
        static const AkUniqueID PLAY_MENU_CANCEL = 3362709232U;
        static const AkUniqueID PLAY_MENU_ERROR = 804361068U;
        static const AkUniqueID PLAY_MENU_GAMESTART = 2088191812U;
        static const AkUniqueID PLAY_MENU_HOVER = 3835684012U;
        static const AkUniqueID PLAY_MENU_SELECT = 2167056030U;
        static const AkUniqueID PLAY_MESSAGE = 716806231U;
        static const AkUniqueID PLAY_MM_MUSIC = 293921080U;
        static const AkUniqueID PLAY_NPC_ATTACK = 1559615426U;
        static const AkUniqueID PLAY_NPC_DEATH = 3888855080U;
        static const AkUniqueID PLAY_NPC_IDLE = 116059476U;
        static const AkUniqueID PLAY_PICKUP = 3860455926U;
        static const AkUniqueID PLAY_PLYR_FS = 1891799159U;
        static const AkUniqueID PLAY_PLYR_VOX__JUMP = 3770688955U;
        static const AkUniqueID PLAY_PLYR_VOX_ATTACK = 1857515512U;
        static const AkUniqueID PLAY_PLYR_VOX_DEATH = 3329526714U;
        static const AkUniqueID PLAY_PLYR_VOX_GRUNT = 1671475754U;
        static const AkUniqueID PLAY_PLYR_VOX_HORN = 766669215U;
        static const AkUniqueID PLAY_PLYR_VOX_SPECIAL = 703173333U;
        static const AkUniqueID STOP_AMBIENT = 3850878248U;
        static const AkUniqueID WIND_BLOW = 3737176454U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace PLAYERSTATUS
        {
            static const AkUniqueID GROUP = 3800848640U;

            namespace STATE
            {
                static const AkUniqueID ALIVE = 655265632U;
                static const AkUniqueID DEATH = 779278001U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace PLAYERSTATUS

    } // namespace STATES

    namespace SWITCHES
    {
        namespace AMB_SWITCH
        {
            static const AkUniqueID GROUP = 439968226U;

            namespace SWITCH
            {
                static const AkUniqueID DESERT = 1850388778U;
                static const AkUniqueID FIELD = 514358619U;
                static const AkUniqueID SNOW = 787898836U;
            } // namespace SWITCH
        } // namespace AMB_SWITCH

        namespace FS_MATERIAL
        {
            static const AkUniqueID GROUP = 20433824U;

            namespace SWITCH
            {
                static const AkUniqueID CONCRETE = 841620460U;
                static const AkUniqueID DIRT = 2195636714U;
                static const AkUniqueID GRASS = 4248645337U;
                static const AkUniqueID GRAVEL = 2185786256U;
                static const AkUniqueID SNOW = 787898836U;
            } // namespace SWITCH
        } // namespace FS_MATERIAL

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID PLAYER_HEALTH = 215992295U;
        static const AkUniqueID PLAYER_SPEED = 1062779386U;
        static const AkUniqueID TIME_LEFT = 3301358078U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAINSOUNDBANK = 534561221U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AMBIENT = 77978275U;
        static const AkUniqueID HUD = 646625284U;
        static const AkUniqueID MAIN_AUDIO_BUS = 2246998526U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID NPC = 662417162U;
        static const AkUniqueID PLAYER = 1069431850U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID REVERB = 348963605U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
