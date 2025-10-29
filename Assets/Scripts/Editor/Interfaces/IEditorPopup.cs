using System.Threading.Tasks;
namespace KJakub.Octave.Editor.Interfaces
{
    public interface IEditorPopup
    {
        public Task<int?> RequestInt();
        public Task<SnappingType?> RequestSnappingType();
    }
}