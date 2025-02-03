using Data.Settings.Language;
using Services;
using Services.PauseService;
using Services.SaveLoad;

public interface IServiceRegister
{
    void  RegisterServices( PauseService pauseService, Language language, AllServices services );
}