using Data.Settings.Language;
using Services;
using Services.Audio;
using Services.PauseService;
using Services.SaveLoad;

public interface IServiceRegister
{
    void  RegisterServices(LoadingCurtain loadingCurtain , Language language, AllServices services,AudioManager audioManager );
}